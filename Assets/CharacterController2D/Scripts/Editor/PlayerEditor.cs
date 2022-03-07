using UnityEditor;
using UnityEngine;

namespace AGGE.CharacterController2D.Editor {
    [CustomEditor(typeof(Player))]
    public class PlayerEditor : UnityEditor.Editor {
        protected void OnSceneGUI() {
            Handles.BeginGUI();

            var player = target as Player;

            GUILayout.Window(
                2,
                new Rect(Screen.width - 220, Screen.height - 130, 40, 100),
                (id) => {
                    EditorGUILayout.LabelField(string.Format("Jump Duration : {0:N3}", player.jumpDuration));
                    EditorGUILayout.LabelField(string.Format("Jump Height     : {0:N3}", player.jumpHeight));
                    EditorGUILayout.LabelField(string.Format("Jump Distance : {0:N3}", player.jumpDistance));
                },
                "Measurements"
            );

            Handles.EndGUI();
        }
    }
}