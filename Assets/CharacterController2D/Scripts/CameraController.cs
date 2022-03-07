using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{

	public float speedX;
	public float speedY;
	public Transform target;

	void Awake () 
	{
		if(target == null)
			target = FindObjectOfType<Player>().transform;

		Vector3 startPos = transform.position;
		Vector3 targetPos = target.position;

		if(speedX > 0)
			startPos.x = targetPos.x;

		if(speedY > 0)
			startPos.y = targetPos.y;

		transform.position = startPos;
	}

	void Update () 
	{
		Vector3 pos = transform.position;
		Vector3 diff = target.position - pos;

		if(speedX > 0 && Mathf.Abs(diff.x) > 0.02f)
			pos.x += diff.x * Time.deltaTime * speedX;
	
		if(speedY > 0 && Mathf.Abs(diff.y) > 0.02f)
			pos.y += diff.y * Time.deltaTime * speedY;

		transform.position = pos;
	}
}
