using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public bool showCurves = true;
	public AudioClip[] jumpSounds;
	public Material[] materials;

	public static GameManager instance;

	private bool initLeftTrigger;
	private AudioSource audioSource;

	void Awake()
	{
		instance = this;
		audioSource = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			int index = Application.loadedLevel+1;

			if(index < Application.levelCount)
				Application.LoadLevel(index);
		}
		else if(Input.GetKeyDown(KeyCode.Q))
		{
			int index = Application.loadedLevel-1;

			if(index >= 0)
				Application.LoadLevel(index);
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				Application.LoadLevel(10);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				Application.LoadLevel(11);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				Application.LoadLevel(12);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				Application.LoadLevel(13);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha5))
			{
				Application.LoadLevel(14);
			}
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				Application.LoadLevel(0);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				Application.LoadLevel(1);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				Application.LoadLevel(2);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				Application.LoadLevel(3);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha5))
			{
				Application.LoadLevel(4);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha6))
			{
				Application.LoadLevel(5);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha7))
			{
				Application.LoadLevel(6);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha8))
			{
				Application.LoadLevel(7);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha9))
			{
				Application.LoadLevel(8);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha0))
			{
				Application.LoadLevel(9);
			}
		}
		if(Input.GetKeyDown(KeyCode.H))
			showCurves = !showCurves;

		float leftTrigger = GetLeftTrigger();
		float timeScale = 1 - (leftTrigger / 1.2f);

		if(timeScale > 0.9f)
			timeScale = 1f;

		if(timeScale < 0.05f)
			timeScale = 0.05f;

		Time.timeScale = timeScale;
	}

	float GetLeftTrigger()
	{		
		float trigger = Input.GetAxisRaw("LeftTrigger");
		
		if(!initLeftTrigger)
		{
			if(trigger == 0)
				return 0;
			else
				initLeftTrigger = true;
		}

		return (trigger - 1) / -2;
	}
		
	public void PlayJumpSound()
	{
		int index = Random.Range(0, jumpSounds.Length);
		audioSource.clip = jumpSounds[index];
		audioSource.Play();
	}
}
