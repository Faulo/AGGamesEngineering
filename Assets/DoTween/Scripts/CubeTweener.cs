using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DoTween.Scripts {
    public class CubeTweener : MonoBehaviour {
        [SerializeField] Vector3 target;
        [SerializeField] Vector3[] waypoints;
        
        protected void Start() {
            SimpleTween();
        }

        void SimpleTween() {
            // Beispiel für einen einfachen Tween eines Wertes
            // DOTween.To(()=> transform.position, x=> transform.position = x, targetPos, 3f);
            // aequivalent mit der erweiterung von transform
            // transform.DOMove(targetPos, 3f);
            // transform.DOJump(transform.position, 1f, 5, 3f);
    
            TweenerCore<Vector3, Vector3, VectorOptions> myTween = null;
            myTween = transform.DOScale(target, 3f)
                .SetLoops(3)
                .SetAutoKill(false)
                .OnComplete(()=>myTween.Rewind());
        }
        
        void SequenceTween() {
            // Grab a free Sequence to use
            Sequence mySequence = DOTween.Sequence();
            // Add a movement tween at the beginning
            mySequence.Append(transform.DOMoveX(5, 1));
            mySequence.Append(transform.DOMoveX(0, 1));
            // Add a rotation tween as soon as the previous one is finished
            mySequence.Append(transform.DORotate(new Vector3(0,180,0), 1));
            // Insert a scale tween for the whole duration of the Sequence
            mySequence.Insert(0, transform.DOScale(new Vector3(3,3,3), mySequence.Duration()));
            // Delay the whole Sequence by 3 seconds
            mySequence.PrependInterval(3);
        }
    }
}