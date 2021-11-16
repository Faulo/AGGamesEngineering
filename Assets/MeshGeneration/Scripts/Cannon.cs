using System.Collections;
using UnityEngine;

namespace AGGE.MeshGeneration {
    public class Cannon : MonoBehaviour {
        [SerializeField]
        Rigidbody projectilePrefab = default;
        [SerializeField, Range(0, 100000)]
        float shootForce = 1000;
        [SerializeField, Range(0, 100000)]
        float shootTorque = 10;
        [SerializeField, Range(0, 100)]
        float shootInterval = 1;

        IEnumerator Start() {
            while (true) {
                Shoot();
                yield return new WaitForSeconds(shootInterval);
            }
        }
        void Shoot() {
            var projectileInstance = Instantiate(projectilePrefab, transform);
            projectileInstance.AddForce(shootForce * transform.forward, ForceMode.VelocityChange);
            projectileInstance.AddTorque(shootTorque * transform.forward, ForceMode.VelocityChange);
        }
    }
}