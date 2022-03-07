using UnityEngine;
using System.Collections;

public class InputArrow : MonoBehaviour 
{
	public Renderer visuals;

	private Vector3 newScale = Vector3.one;

	void Update () 
	{
		bool visible = false;

		if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
		{
			newScale.x = -1;
		}

		if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			newScale.x = 1;
		}

		if(GameManager.instance.showCurves)
			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
				visible =true;

		visuals.enabled = visible;

		transform.localScale = newScale;
	}
}
