using System;
using System.Collections;
using UnityEngine;

namespace Coroutines.Scripts {
    public class Turret : MonoBehaviour {
        [SerializeField] private bool canSpawn;
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private float heatUpInSeconds;
        [SerializeField] private float coolDownInSeconds;
        [SerializeField] private Vector3 spawnForce;
        private Coroutine spawnCoroutine;

        protected void Start() {
            spawnCoroutine = StartCoroutine(Spawn_Co());

            StartCoroutine(ActionAfterSeconds_Co(Spawn, 1));

        }

        protected void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                canSpawn = !canSpawn;
            }
        }

        private IEnumerator Spawn_Co() {
            while (true) {
                yield return new WaitUntil(() => canSpawn);
                yield return HeatUp_Co();
                yield return ActionAfterSeconds_Co(() => Spawn(), 1f);
                yield return CoolDown_Co();
            }
        }

        private IEnumerator HeatUp_Co() {
            Debug.Log("Started heating up!");
            yield return new WaitForSeconds(heatUpInSeconds);
            yield return new WaitUntil(() => canSpawn);
            Debug.Log("Finished heating up!");
        }

        private IEnumerator CoolDown_Co() {
            Debug.Log("Started cooling down!");
            yield return new WaitForSeconds(coolDownInSeconds);
            Debug.Log("Finished cooling down!");
        }

        private void Spawn() {
            var obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            if (obj.TryGetComponent(out Rigidbody rb)) {
                rb.AddForce(spawnForce, ForceMode.VelocityChange);
            }
            Debug.Log("Spawned Bullet");
        }





        private IEnumerator ActionAfterSeconds_Co(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}