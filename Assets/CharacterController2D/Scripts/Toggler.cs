using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toggler : MonoBehaviour 
{
	public enum Type { SingleGroup, Toggle }
	public Type type = Type.SingleGroup;

	public ToggleGroup[] groups;
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Z))
	    {
			ChangeGroup(0);
		}
		else if(Input.GetKeyDown(KeyCode.X))
		{
			ChangeGroup(1);
		}
		else if(Input.GetKeyDown(KeyCode.C))
		{
			ChangeGroup(2);
		}
		else if(Input.GetKeyDown(KeyCode.V))
		{
			ChangeGroup(3);
		}
		else if(Input.GetKeyDown(KeyCode.B))
		{
			ChangeGroup(4);
		}
		else if(Input.GetKeyDown(KeyCode.N))
		{
			ChangeGroup(5);
		}
		else if(Input.GetKeyDown(KeyCode.M))
		{
			ChangeGroup(6);
		}
	}

	void ChangeGroup (int index)
	{
		int length = groups.Length;

		if(index >= length)
			return;

		if(type == Type.SingleGroup)
		{
			for(int i = 0; i < length; i++)
			{
				ToggleGroup group = groups[i];
				
				if(group != null)
					group.Toggle(i == index);
			}
		}
		else if(type == Type.Toggle)
		{
			ToggleGroup group = groups[index];

			if(group != null)
				group.Swap();
		}

	}

	[System.Serializable]
	public class ToggleGroup
	{
		public GameObject holder;

		public bool changeBackground;
		public Color background;

		public CameraController cameraController;
		public Transform cameraTarget;

		public void Toggle(bool value)
		{
			if(value)
			{
				if(changeBackground)
					Camera.main.backgroundColor = background;

				if(cameraController && cameraTarget)
					cameraController.target = cameraTarget;
			}

			if(holder)
				holder.SetActive(value);				
		}

		public void Swap()
		{
			if(holder)
				holder.SetActive(!holder.activeSelf);

			if(cameraController && cameraTarget)
				cameraController.target = cameraTarget;
		}
	}
}
