using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
	void OnSceneGUI()
	{
		Handles.BeginGUI();

		Player player = this.target as Player;

		GUILayout.Window(2, new Rect(Screen.width-220, Screen.height-130, 40, 100), (id)=>{
			// Content of window here
			//GUI.Label(new Rect(0, 0, 100, 100), "Testing");

			EditorGUILayout.LabelField(string.Format("Jump Duration : {0:N3}", player.jumpDuration));
			EditorGUILayout.LabelField(string.Format("Jump Height     : {0:N3}", player.jumpHeight));
			EditorGUILayout.LabelField(string.Format("Jump Distance : {0:N3}", player.jumpDistance));

		}, "Measurements");


		Handles.EndGUI();
	}
}