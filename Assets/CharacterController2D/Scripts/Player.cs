using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public enum ChangeType {Additive, WeightedAverage, Instant}

	[Header("----- Ground Movement -----")]
	public float acceleration = 100f;
	public float deceleration = 50f;
	public float dirChangeAcceleration = 30;
	public float maxVelocity = 17f;

	[Header("-- Details --")]
	public AnimationCurve analogMaxVelocity;
	public ChangeType accelerationType;
	public ChangeType decelerationType;
	public ChangeType dirChangeType;

	[Space(20)]
	[Header("----- Jump -----")]
	public float gravity = 5f;
	public float terminalVelocity = 99f;
	public Vector3 jumpTakeoff;

	[Header("-- Details --")]
	public bool absoluteTakeoff;
	public float minimumJumpTime = 0.1f;
	public bool holdingJumpAllowed;
	
	[Header("- Jump, Additional Horizontal Velocity -")]
	public Vector3 jumpTakeoffAdd;
	public float jumpTakeoffAddVelocity;
	public float jumpHorizontalVelocityFactor;
	
	[Header("- Jump Short -")]
	public float jumpShortDistance;
	public float jumpShortPenalty;
	public float minVelocityAirShort;

	[Header("- Jump Upwards -")]
	public bool useJumpUpwards;
	public float jumpTakeoffUpwards;
	public Vector3 dragGoingUpwards;
	public Vector3 dragOnJumpReleaseUpwards;


	//[Space(20)]

	[Header("- Jump Hold -")]
	[Header("----- Air Control, Jump Input -----")]

	public float jumpForceAdditive;
	public float jumpForceAdditiveMinimum;
	
	[Header("- Jump Release -")]
	public bool instantBreakOnRelease;
	public Vector3 dragOnJumpRelease;

	[Space(20)]
	[Header("----- Air Control, Horizontal Input -----")]
	public float accelerationAir = 150f;
	public float decelerationAir;
	public float airDirChangeAcceleration;
	public float maxVelocityAir = 17.5f;

	[Header("- Horizontal Hold -")]
	public float minVelocityAirHold;
	public float minVelocityAirHoldInputFactor;
	public bool dampDirectionChange;

	[Header("- Horizontal Release -")]
	public float minVelocityAirRelease;
	public Vector3 decelerationAirReleaseHorizontal;
	public bool preventHorizontalRepress;

	[Header("---------------------------------------")]

	[Header("----- Details -----")]

	public float collisionWidth = 0.5f;
	public float collisionHeight = 1f;

	public float deadZoneX = 0.5f;
	public float deadZoneAirX = 0.5f;

	public float wallSlideFactor;	
	public float edgeJump;
	public float jumpCache;
	public bool removeVerticalVelocity;

	[Header("- Delays -")]
	public float jumpDelay;
	public float jumpLandDelay;
	public float jumpReleaseDelay;

	[Header("---------------------------------------")]

	[Header("--- Measurements ---")]
	public float jumpDuration;
	public float jumpHeight;
	public float jumpDistance;
	public bool grounded = true;
	public bool falling;
	public bool jumpingUpwards;
	public bool jumpingShort;
	public float runDistance;


	[Header("--- System ---")]
	[Space(10)]
	public LayerMask defaultMask;
	public LayerMask platformMask;
	public Transform visuals;
	public InputSimulator simulator;

	//State
	private float dt;
	private Vector3 velocity;
	private Vector3 currentPos;
	private float currentMaxVelocity;

	private bool previousJump = false;
	private Vector3 previousVelocity;
	private Vector3 previousPos;
	private float previousTime;
	private Vector3 pastPos;
	private float pastTime;
	private Vector3 takeoffPos;

	private bool horizontalInputReleased;
	private bool skipVerticalDrag;
	private Vector3 nextPos;
	private float jumpDir;

	private float jumpDelayTimer;
	private float jumpLandDelayTimer;
	private float jumpReleaseDelayTimer;
	private float fallTimer;
	private float airTimer;
	private float jumpCacheTimer;

	//Collision
	private float rayOffset = 0.002f;
	private float collisionWidthHalf;
	private float collisionHeightHalf;

	//Input
	private bool jump;
	private float inputX;

	void Start () 
	{
		Setup();
	}

	public void Setup()
	{
		currentPos = transform.position;
		previousPos = currentPos;
		pastPos = currentPos;
	}

	void Update () 
	{
		if(pastTime == 0)
		{
			visuals.position = pastPos;
		}
		else
		{
			//Interpolate visuals
			float dt = Time.fixedDeltaTime;
			float timeFraction = ((Time.time - dt) - pastTime) / dt;
			Vector3 nextPos = pastPos + (previousPos - pastPos) * timeFraction;
			visuals.position = nextPos;
		}
	}

	void FixedUpdate () 
	{
		dt = Time.deltaTime;

		UpdateInput();		 
		UpdateCharacter();
	}
	
	void UpdateCharacter () 
	{
		if(falling)
			fallTimer += dt;

		//Jump

		//Delay Jump Release
		if(!jump && jumpReleaseDelayTimer > 0 && velocity.y > 0)
		{
			jumpReleaseDelayTimer -= dt;

			if(jumpReleaseDelayTimer > 0)
				jump = true;
		}

		//Land Delay
		if(jumpLandDelayTimer > 0)
		{
			jump = false;
			jumpLandDelayTimer -= dt;
		}

		//Initiate Jump
		bool startJump = jump && (!previousJump || holdingJumpAllowed);

		//Jump Cache
		if(jumpCache != 0)
		{
			if(startJump)
				jumpCacheTimer = jumpCache;

			if(jumpCacheTimer > 0)
			{
				jumpCacheTimer -= dt;
				startJump = true;
			}
		}

		//Delayed Jump
		if(jumpDelayTimer > 0)
		{
			jumpDelayTimer -= dt;
			
			if(jumpDelayTimer <= 0)
				Jump ();
		}

		bool allowEdgeJump = falling && edgeJump > 0 && fallTimer < edgeJump;

		if(startJump && (grounded || allowEdgeJump))
		{
			//Initiate Jump

			//Jump Delay
			if(jumpDelayTimer <= 0)
			{
				if(jumpDelay > 0)
					jumpDelayTimer = jumpDelay;

				//Land Delay
				jumpDelayTimer += jumpLandDelayTimer;
			}

			//Regular Jump
			if(jumpDelayTimer <= 0)
				Jump ();
		}

		if(skipVerticalDrag)
		{
			skipVerticalDrag = false;
		}
		else
		{
			//Jump Force Additive
			if(jump && velocity.y > jumpForceAdditiveMinimum)
				velocity.y += jumpForceAdditive * dt;

			if(jumpingUpwards)
			{
				//Drag upwards jump
				if(velocity.y > 0)
				{
					if(!jump)
					{
						//Drag on release
						velocity += dragOnJumpReleaseUpwards * dt;
					} 
					else
					{
						//Drag going upwards
						velocity += dragGoingUpwards * dt; 
					}
				}
			}
			else
			{
				//Drag forwards jump
				if(!jump && velocity.y > 0)
					velocity += dragOnJumpRelease * dt; 			
			}
			
			//Gravity
			velocity.y -= gravity * dt;
			
			//Terminal Velocity
			if(terminalVelocity != 0 && velocity.y < -terminalVelocity)
				velocity.y = -terminalVelocity;
			
			//Jump break
			if(instantBreakOnRelease && !jump && velocity.y > 0 && airTimer > minimumJumpTime)
			{
				velocity.y = 0;
			}
		}

		//Movement
		if(grounded)
		{
			GroundControl();
		}
		else
		{
			AirControl();
		}
		
		//Wall Slide
		//if(previousWallRight && inputX > deadZoneX && velocity.y > 0)
		//	velocity.y += velocity.y * 0.5f * dt;
		
		//Apply Velocity
		nextPos = currentPos + velocity * dt;
		
		CollisionDetection();

		//Set Position
		transform.position = nextPos;
		
		currentPos = transform.position;

		if(grounded)
		{
			float dist = (nextPos - previousPos).magnitude;
			runDistance += dist;
		}
		
		if(currentPos.y > jumpHeight)
			jumpHeight = currentPos.y;
		
		previousJump = jump;
		previousVelocity = velocity;
		
		pastPos = previousPos;
		pastTime = previousTime;
		
		previousPos = nextPos;
		previousTime = Time.time;
		//previousWallRight = wallRightDown;
	}

	void Jump ()
	{
		//Takeoff Velocity

		if (useJumpUpwards && Mathf.Abs(inputX) < deadZoneX) 
		{
			//Jump Upwards
			velocity.x = 0;
			velocity.y += jumpTakeoffUpwards;
			jumpingUpwards = true;
		}
		else 
		{
			//Regular Jump

			Vector3 velocityBeforeTakeoff = velocity;
			jumpDir = Mathf.Sign(inputX);

			if(absoluteTakeoff)
			{
				//Absolute
				velocity.x = jumpDir * jumpTakeoff.x;
				velocity.y = jumpTakeoff.y;
			}
			else
			{
				//Additive
				velocity.x += Mathf.Sign (velocity.x) * jumpTakeoff.x;
				velocity.y += jumpTakeoff.y;
			}

			//Add
			if (Mathf.Abs (velocityBeforeTakeoff.x) > jumpTakeoffAddVelocity)
				velocity += jumpTakeoffAdd;

			float addVelocityFactor = jumpHorizontalVelocityFactor;

			if(jumpShortDistance > 0 && runDistance < jumpShortDistance)
			{
				jumpingShort = true;
				addVelocityFactor -= jumpShortPenalty;
			}
			else
			{
				jumpingShort = false;
			}

			velocity.y += Mathf.Abs(velocityBeforeTakeoff.x) * addVelocityFactor;
			
			jumpingUpwards = false;
		}

		if(Mathf.Abs(inputX) > deadZoneX)
			horizontalInputReleased = false;

		jumpDuration = 0;
		jumpHeight = 0;
		airTimer = 0;
		jumpReleaseDelayTimer = jumpReleaseDelay;
		takeoffPos = previousPos;
		grounded = false;
		falling = false;
		skipVerticalDrag = true;

		if(Application.isPlaying)
			GameManager.instance.PlayJumpSound();
	}

	void Land()
	{
		if(!grounded)
		{
			jumpDistance = Mathf.Abs(previousPos.x - takeoffPos.x);
			jumpLandDelayTimer = jumpLandDelay;
			runDistance = 0;
		}

		if(falling)
		{
			falling = false;
		}

		velocity.y = 0;
		grounded = true;
	}

	void GroundControl()
	{		
		//Max Velocity
		currentMaxVelocity = maxVelocity;
		
		if(analogMaxVelocity.length > 0 && Mathf.Abs(inputX) > deadZoneX)
			currentMaxVelocity = analogMaxVelocity.Evaluate(Mathf.Abs(inputX)) * maxVelocity;

		//Movement
		if(Mathf.Abs(inputX) < deadZoneX)
		{
			//--//Deceleration
			if(decelerationType == ChangeType.WeightedAverage)
			{
				//Weighted Average
				float decelerateForce = velocity.x * dt * deceleration;
				
				if(Mathf.Abs(decelerateForce) > Mathf.Abs(velocity.x))
					velocity.x = 0;
				else
					velocity.x -= velocity.x * dt * deceleration; 
			}
			else if(decelerationType == ChangeType.Additive)
			{
				//Additive
				float deceleratForce = Mathf.Sign(velocity.x) * deceleration * dt;
				
				if(Mathf.Abs(deceleratForce) > Mathf.Abs(velocity.x))
					velocity.x  = 0;
				else
					velocity.x -= deceleratForce;
				
			}
			else if(decelerationType == ChangeType.Instant)
			{
				//Instant
				velocity.x = 0;
			}
			
			runDistance = 0;
		}
		else
		{
			if(velocity.x != 0 && Mathf.Sign(velocity.x) != Mathf.Sign(inputX))
			{
				//Direction Change
				if(dirChangeType == ChangeType.WeightedAverage)
				{
					//Weighted Average
					float targetVelocity = Mathf.Sign(inputX) * currentMaxVelocity;
					velocity.x += (targetVelocity - velocity.x) * dt * dirChangeAcceleration; 
				}
				else if(dirChangeType == ChangeType.Additive)
				{
					//Additive
					velocity.x += Mathf.Sign(inputX) * dirChangeAcceleration * dt;
				}
				else if(dirChangeType == ChangeType.Instant)
				{
					//Instant
					velocity.x = 0;
				}
			}
			else
			{
				//--//Acceleration
				if(accelerationType == ChangeType.WeightedAverage)
				{
					//Weighted Average
					float targetVelocity = Mathf.Sign(inputX) * currentMaxVelocity;
					velocity.x += (targetVelocity - velocity.x) * dt * acceleration; 
				}
				else if(accelerationType == ChangeType.Additive)
				{
					//Additive
					velocity.x += Mathf.Sign(inputX) * acceleration * dt;
				}
				else if(accelerationType == ChangeType.Instant)
				{
					//Instant
					velocity.x = Mathf.Sign(inputX) * currentMaxVelocity;
				}
			}
		}

		if(Mathf.Abs(velocity.x) >= currentMaxVelocity)
			velocity.x = Mathf.Sign(velocity.x) * currentMaxVelocity;
	}
	
	void AirControl()
	{
		airTimer += dt;
		jumpDuration += dt;

		if(jumpingUpwards)
			return;

		if(horizontalInputReleased || Mathf.Abs(inputX) < deadZoneAirX || (dampDirectionChange && Mathf.Sign(inputX) != jumpDir))
		{
			if(preventHorizontalRepress)
				horizontalInputReleased = true;

			//Drag
			if(Mathf.Abs(velocity.x) > minVelocityAirRelease)
				velocity.x -= Mathf.Sign(velocity.x) * decelerationAirReleaseHorizontal.x * dt;
			
			if(Mathf.Abs(velocity.x) < minVelocityAirRelease && Mathf.Abs(velocity.x) > 0)
				velocity.x = minVelocityAirRelease * Mathf.Sign(velocity.x);
		}
		else
		{
			//Drag
			float currentMinVelocity = minVelocityAirHold + (Mathf.Abs(inputX) * minVelocityAirHoldInputFactor);

			if(jumpingShort)
				currentMinVelocity = minVelocityAirShort;

			if(Mathf.Abs(velocity.x) > currentMinVelocity)
				velocity.x -= Mathf.Sign(velocity.x) * decelerationAir * dt;

			if(Mathf.Abs(velocity.x) < currentMinVelocity && velocity.x != 0)
				velocity.x = currentMinVelocity * Mathf.Sign(velocity.x);

			//Acceleration
			if(velocity.x != 0 && Mathf.Sign(velocity.x) != Mathf.Sign(inputX))
			{
				velocity.x += Mathf.Sign(inputX) * airDirChangeAcceleration * dt;
			}
			else
			{
				velocity.x += Mathf.Sign(inputX) * accelerationAir * dt;
			}
		}

		//Max Horizontal Velocity
		if(Mathf.Abs(velocity.x) >= maxVelocityAir)
			velocity.x = Mathf.Sign(velocity.x) * maxVelocityAir;
	}

	void CollisionDetection ()
	{
		//Collision
		collisionWidthHalf = collisionWidth / 2;
		collisionHeightHalf = collisionHeight / 2;

		//Vertical
		bool downRight = CollisionCheckGround (1);
		bool downLeft = CollisionCheckGround (-1);

		CollisionCheck (1, 1, Vector2.up);
		CollisionCheck (-1, 1, Vector2.up);

		//Horizontal
		CollisionCheck (1, -1, Vector2.right);
		CollisionCheck (1, 1, Vector2.right);

		CollisionCheck (-1, -1, -Vector2.right);
		CollisionCheck (-1, 1, -Vector2.right);		

		//Walked off edge
		if (!downLeft && !downRight && grounded)
		{
			grounded = false;
			falling = true;
			fallTimer = 0;
		}
	}

	private bool CollisionCheckGround(int offsetX)
	{
		Vector2 origin;
		origin.x = previousPos.x + (collisionWidthHalf - rayOffset) * offsetX;
		origin.y = previousPos.y + (collisionHeightHalf - rayOffset) * -1;

		RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, 3f, platformMask);
		
		if(hit)
		{
			//Platform check
			if(hit.point.y < hit.transform.position.y + hit.collider.bounds.extents.y - 0.05f)
				return false;

			float limitY = hit.point.y + collisionHeightHalf;
			
			if(nextPos.y < limitY)
			{
				nextPos.y = limitY;

				Land();
				return true;
			}
					
			//Debug.DrawLine(origin, hit.point, Color.red);
			//return false;
		}

		//Debug.DrawRay(origin, dir, Color.red);
		return false;
	}

	private bool CollisionCheck(int offsetX, int offsetY, Vector2 dir)
	{
		Vector2 origin;
		origin.x = previousPos.x + (collisionWidthHalf - rayOffset) * offsetX;
		origin.y = previousPos.y + (collisionHeightHalf - rayOffset) * offsetY;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir , 3f, defaultMask);
		
		if(hit)
		{
			if(dir.x != 0)
			{
				//Horizontal
				float limitX = hit.point.x + collisionWidthHalf * -dir.x;

				if(nextPos.x * dir.x > limitX * dir.x)
				{
					nextPos.x = limitX;

					//wall slide
					if(wallSlideFactor > 0)
						velocity.y += velocity.x * wallSlideFactor;

					if(removeVerticalVelocity && velocity.y > 0 && Mathf.Abs(previousVelocity.x) > 0)
						velocity.y = 0;

					velocity.x = 0;
					return true;
				}
			}
			else
			{
				//Vertical
				float limitY = hit.point.y + collisionHeightHalf * -dir.y;
				
				if(nextPos.y * dir.y > limitY * dir.y)
				{
					nextPos.y = limitY;
					velocity.y = 0;				
					return true;
				}
			}

			//Debug.DrawLine(origin, hit.point, Color.red);

		}

		//Debug.DrawRay(origin, dir, Color.red);
		return false;
	}

	void UpdateInput()
	{
		if(simulator && simulator.state != InputSimulator.State.None)
		{
			inputX = simulator.currentStickX;
			jump = simulator.currentJump;
		}
		else
		{
			inputX = 0;
			
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				inputX =  -1;
			}
			else if(Input.GetKey(KeyCode.RightArrow))
			{
				inputX =  1;
			}
			else
			{
				inputX = Input.GetAxisRaw("Horizontal");
			}
			
			jump = Input.GetButton("Jump") || Input.GetKey(KeyCode.UpArrow);
		}
	}

	//Simulating jump curve in editor

	public void SetDeltaTime(float value)
	{
		dt = value;
	}

	public Vector3 GetPosition()
	{
		return currentPos;
	}

	public void SetPosition(Vector3 p)
	{
		transform.position = p;
		currentPos = transform.position;
		previousPos = currentPos;

		velocity = Vector3.zero;
		previousVelocity = Vector3.zero;

		Setup();
	}

	public bool GetJumpForceAdditive()
	{
		if(jump && velocity.y > jumpForceAdditiveMinimum)
			return true;
		else
			return false;
	}

	public void Simulate(float horizontalInput, bool jumpInput)
	{
		inputX = horizontalInput;
		jump = jumpInput;

		UpdateCharacter();
	}

	public void Reset (Vector3 resetPos)
	{
		transform.position = resetPos;
		velocity = Vector3.zero;
		previousVelocity = Vector3.zero;
		
		airTimer = 0;
		grounded = false;
		falling = false;
		runDistance = 0;
		jumpCacheTimer = 0;

		jump = false;
		previousJump = false;
		dt = 0;
		inputX = 0;
	}
}
