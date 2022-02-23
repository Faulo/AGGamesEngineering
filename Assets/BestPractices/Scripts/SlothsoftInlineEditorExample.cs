using UnityEngine;
using Slothsoft.UnityExtensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AGGE.BestPractices {
    public class SlothsoftInlineEditorExample : MonoBehaviour {

        void DoStuff() {
            Debug.Log("Hallo Welt");
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(SlothsoftInlineEditorExample))]
        class InlineEditorExampleEditor : RuntimeEditorTools<SlothsoftInlineEditorExample> {
            protected override void DrawEditorTools() {
                DrawButton("Click Me", target.DoStuff);
            }
        }
#endif
    }
}
