using UnityEngine;

namespace AGGE.CleanCode {
    public interface IMover {
        void Move(Vector3 direction, float speed);
    }
}