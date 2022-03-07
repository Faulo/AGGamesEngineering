using UnityEngine;

namespace AGGE.BestPractices.DesignPatterns {
    public interface IMover {
        void Move(Vector3 direction, float speed);
    }
}