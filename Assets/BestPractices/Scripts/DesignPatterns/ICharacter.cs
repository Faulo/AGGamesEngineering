using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    public interface ICharacter {
        public float speed { get; set; }
        public Transform transform { get; set; }
        void Start();
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update();
    }
    public interface IExplodable {
        void Explode();
    }
}