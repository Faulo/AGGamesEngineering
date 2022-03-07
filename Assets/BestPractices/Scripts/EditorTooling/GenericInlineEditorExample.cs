using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AGGE.BestPractices.EditorTooling {
    public class GenericInlineEditorExample : MonoBehaviour {

        void DoStuff() {
            Debug.Log("Hallo Welt");
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(GenericInlineEditorExample))]
        class InlineEditorExampleEditor : Editor {
            new GenericInlineEditorExample target => base.target as GenericInlineEditorExample;
            public override void OnInspectorGUI() {
                DrawDefaultInspector();
                if (GUILayout.Button("Click Me")) {
                    target.DoStuff();
                }
            }
        }
#endif
    }
}
