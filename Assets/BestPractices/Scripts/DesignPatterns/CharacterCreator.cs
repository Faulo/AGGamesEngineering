using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    public class CharacterCreator : MonoBehaviour {
        [SerializeField]
        string type = "avatar";
        [SerializeField]
        float speed = 1;
        [SerializeField]
        bool useOverrideTransform = false;
        [SerializeField]
        Transform overrideTransform = default;

        ICharacter behaviour;

        // Start is called before the first frame update
        protected void Awake() {
            behaviour = CharacterFactory.CreateCharacter(type, transform);
            behaviour.speed = speed;
            if (useOverrideTransform) {
                behaviour.transform = overrideTransform;
            }
        }

        protected void Start() {
            behaviour.Start();
        }

        // Update is called once per frame
        protected void Update() {
            behaviour.Update();
        }
    }
}