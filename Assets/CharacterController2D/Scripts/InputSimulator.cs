using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSimulator : MonoBehaviour 
{
	public DrawCurve curve;
	public Player player;
	public bool active;

	public float runBeforeJump = 0.5f;
	public float jumpDuration = 0.2f;
	public float runAfterJump = 0.3f;
	public float completeDelay = 1f;
	public float maxRunTime = 5f;
	public float stickX = 1f;
	public float airStickDelay;
	public float stickXAir = 1f;
	public bool drawJumpRelase = true;	
	public float jumpReleaseLineSize = 0.5f;

	private float abortTimer;
	private float timer;
	private float completeTimer;
	private Vector3 originalPos;

	public enum State { None, Falling, BeforeJump, Jump, Air, AfterJump, Complete }
	public State state = State.None;

	[HideInInspector]
	public float currentStickX;

	[HideInInspector]
	public bool currentJump;

	private Vector3 playerStartPos;
	private bool previousForceAdditive;

	void OnDrawGizmos() 
	{
		if(!active || !curve || Application.isPlaying)
			return;

		originalPos = player.transform.position;
		player.Setup();
		curve.Clear();

		abortTimer = 0;
		player.SetDeltaTime(Time.fixedDeltaTime);

		Simulate();

		player.Reset(originalPos);
	}

	void Start()
	{
		playerStartPos = player.transform.position;
	}

	void SetFallingState()
	{
		state = State.Falling;
	}

	void FallingState()
	{
		currentStickX = 0;
		currentJump = false;

		if(player.grounded)
		{
			SetBeforeJumpState();
		}
	}

	void SetBeforeJumpState()
	{
		timer = runBeforeJump;
		state = State.BeforeJump;
	}

	void BeforeJumpState()
	{
		timer -= Time.deltaTime;

		currentStickX = stickX;
		currentJump = false;

		if(timer <= 0)
		{
			if(jumpDuration <= 0)
				SetAfterJumpState();
			else
				SetJumpState();
		}
	}

	void SetJumpState()
	{
		timer = jumpDuration;

		state = State.Jump;
	}

	void JumpState()
	{
		//*** need air stick here for limbo and analog controls

		timer -= Time.deltaTime;

		currentStickX = stickX;
		currentJump = true;

		if(timer <= 0)
		{
			SetAirState();
		}
	}

	void SetAirState()
	{
		state = State.Air;
	}

	void AirState()
	{
		currentStickX = stickX;
		currentJump = false;

		if(player.grounded)
		{
			SetAfterJumpState();
		}
	}

	void SetAfterJumpState()
	{
		timer = runAfterJump;

		state = State.AfterJump;
	}

	void AfterJumpState()
	{
		timer -= Time.deltaTime;

		currentStickX = stickX;
		currentJump = false;

		if(timer <= 0)
		{
			SetCompleteState();
		}
	}

	void SetCompleteState()
	{
		currentStickX = 0;
		currentJump = false;

		if(completeDelay < 0)
		{
			state = State.None;
		}
		else
		{
			completeTimer = completeDelay;	
			state = State.Complete;
		}
	}

	void CompleteState()
	{
		completeTimer -= Time.deltaTime;

		if(completeTimer <= 0)
		{
			player.SetPosition(playerStartPos);
			timer = 0;
			state = State.None;
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(state == State.None || state == State.Complete)
			{				
				player.SetPosition(playerStartPos);
				timer = 0;
				
				SetFallingState();
			}
		}

		if(curve && Input.GetKeyDown(KeyCode.H))
		{
			if(curve.line)
				curve.line.enabled = !curve.line.enabled;

			if(curve.lineRelease)
				curve.lineRelease.enabled = !curve.lineRelease.enabled;
		}
	}

	void FixedUpdate()
	{
		switch(state)
		{
			case State.AfterJump :
				AfterJumpState();
				break;
			case State.Air :
				AirState();
				break;
			case State.BeforeJump:
				BeforeJumpState();
				break;
			case State.Falling:
				FallingState();
				break;
			case State.Jump:
				JumpState();
				break;
			case State.Complete:
				CompleteState();
				break;
		}
	}

	void Simulate()
	{
		//bool test = false;
		//Fall
		while(!player.grounded)
		{
			if(AbortCheck()) 
				return;
			
			player.Simulate(0, false);
			curve.AddPoint();
		}
		
		//Run before jump
		timer = runBeforeJump;
		while(timer > 0)
		{
			timer -= Time.fixedDeltaTime;
			player.Simulate(stickX, false);
			curve.AddPoint();
		}
		
		//Jump inital
		timer = jumpDuration;
		previousForceAdditive = false;

		Vector3 p2 = player.GetPosition();
		//Debug.DrawLine(p2 + Vector3.down * 0.5f, p2 + Vector3.up * 0.5f);
		/*if(!test && player.velocity.y > 1 && player.velocity.y < player.jumpForceAdditiveMinimum)
		{
			test = true;
			Debug.Log ("found");
			Vector3 p4 = player.GetPosition();
			Debug.DrawLine(p4 + Vector3.down * 0.5f, p4 + Vector3.up * 0.5f);
		}*/

		if(curve)
			curve.SetJumpLine(p2);

		while(timer > 0)
		{	


			timer -= Time.fixedDeltaTime;

			if(timer < (jumpDuration - airStickDelay))
			{
				player.Simulate(stickXAir, true);
			}
			else
			{
				player.Simulate(stickX, true);
			}

			VelocityCheck();

			curve.AddPoint();
		}

		Vector3 p = player.GetPosition();

		if(drawJumpRelase)
			Debug.DrawLine(p + Vector3.down * jumpReleaseLineSize, p + Vector3.up * jumpReleaseLineSize);

		if(curve)
			curve.SetJumpReleaseLine(p, jumpReleaseLineSize);
	
		//Wait for landing
		while(!player.grounded)
		{		
			if(AbortCheck())
				return;

			//Any reason not to use stickXAir here?
			//player.Simulate(stickX, false); 

			player.Simulate(stickXAir, false);
			curve.AddPoint();

			VelocityCheck();
		}


		//Run after jump
		timer = runAfterJump;
		while(timer > 0)
		{			
			timer -= Time.fixedDeltaTime;
			player.Simulate(stickX, false);
			curve.AddPoint();
		}
	}

	void VelocityCheck()
	{
		if(curve.lineVelocity)
		{
			bool currentForceAdditvite = player.GetJumpForceAdditive();
			
			if(previousForceAdditive && !currentForceAdditvite)
				curve.SetVelocityLine(player.GetPosition());
			
			previousForceAdditive = currentForceAdditvite;
		}
	}

	bool AbortCheck()
	{
		abortTimer += Time.fixedDeltaTime;
		return abortTimer > maxRunTime;
	}
}
