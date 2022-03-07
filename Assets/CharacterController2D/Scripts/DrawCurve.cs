using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawCurve : MonoBehaviour 
{
	public Transform target;
	public float maxLength = 200;
	public float interval = 0.3f;
	public bool active = true;
	public bool draw = true;

	public Color startColor = Color.green;
	public bool gradientColor = false;
	public Color endColor = Color.red;
	public bool bakeLine;
	public LineRenderer line;
	public LineRenderer lineRelease;
	public LineRenderer lineVelocity;
	public LineRenderer lineJump;
	public float depth = 0.1f;

	private List<Vector3> path = new List<Vector3>();	
	private float timer;

	public void OnEnable()
	{
		if(GameManager.instance)
		{
			bool showCurves = GameManager.instance.showCurves;

			if(line)
				line.enabled = showCurves;
			
			if(lineRelease)
				lineRelease.enabled = showCurves;
		}
	}

	public void Clear()
	{
		path.Clear();
	}

	void Update () 
	{
		if(!active)
			return;

		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			timer = interval;

			AddPoint();
		}

		bool showCurves = GameManager.instance.showCurves;

		if(line)
			line.enabled = showCurves;

		if(lineRelease)
			lineRelease.enabled = showCurves;

		if(lineJump)
			lineJump.enabled = showCurves;
	}

	public void AddPoint()
	{
		path.Add(target.position);
		
		int count = path.Count;
		
		if(count > maxLength)
		{
			path.RemoveAt(0);
		}
	}

	public void SetJumpLine(Vector3 jumpPos)
	{
		if(lineJump)
		{
			lineJump.positionCount = 2;
			Vector3 p1 = jumpPos + Vector3.down * 1f;
			Vector3 p2 = jumpPos + Vector3.up * 1f;
			
			lineJump.SetPosition(0, p1);
			lineJump.SetPosition(1, p2);
		}
	}

	public void SetJumpReleaseLine(Vector3 jumpReleasePos, float size)
	{
		if(lineRelease)
		{
			lineJump.positionCount = 2;
			Vector3 p1 = jumpReleasePos + Vector3.down * size;
			Vector3 p2 = jumpReleasePos + Vector3.up * size;

			p1.z = depth;
			p2.z = depth;

			lineRelease.SetPosition(0, p1);
			lineRelease.SetPosition(1, p2);
		}
	}

	public void SetVelocityLine(Vector3 velocityPos)
	{
		if(lineVelocity)
		{
			lineJump.positionCount = 2;
			Vector3 p1 = velocityPos + Vector3.down * 1f;
			Vector3 p2 = velocityPos + Vector3.up * 1f;

			p1.z = depth;
			p2.z = depth;

			lineVelocity.SetPosition(0, p1);
			lineVelocity.SetPosition(1, p2);
		}
	}

	void OnDrawGizmos()
	{
		int count = path.Count;

		if(count < 2)
			return;

		Vector3 currentPos;

		if(draw)
		{
			Vector3 previousPos = path[0];

			for(int i = 1; i < count; i++)
			{
				currentPos = path[i];

				Color color = startColor;

				if(gradientColor)
					color = Color.Lerp(startColor, endColor, (float)i / count);

				Debug.DrawLine(previousPos, currentPos, color);

				previousPos = currentPos;
			}
		}

		if(!Application.isPlaying && line)
		{
			lineJump.positionCount = count;

			for (int i = 0; i < count; i++)
			{
				currentPos = path[i];
				currentPos.z += depth;

				line.SetPosition(i, currentPos);
			}
		}
	}
}
