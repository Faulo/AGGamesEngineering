using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace AGGE.CharacterController2D {
    public class GameManager : MonoBehaviour {
        public bool showCurves = true;
        public AudioClip[] jumpSounds;
        public Material[] materials;

        public static GameManager instance;

        bool initLeftTrigger;
        AudioSource audioSource;

        protected void Awake() {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        protected void Update() {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.H)) {
                showCurves = !showCurves;
            }

            float leftTrigger = GetLeftTrigger();
            float timeScale = 1 - (leftTrigger / 1.2f);

            if (timeScale > 0.9f) {
                timeScale = 1f;
            }

            if (timeScale < 0.05f) {
                timeScale = 0.05f;
            }

            Time.timeScale = timeScale;
        }

        float GetLeftTrigger() {
            float trigger = Gamepad.current == null
                ? 0
                : Gamepad.current.leftTrigger.ReadValue();

            if (!initLeftTrigger) {
                if (trigger == 0) {
                    return 0;
                } else {
                    initLeftTrigger = true;
                }
            }

            return (trigger - 1) / -2;
        }

        public void PlayJumpSound() {
            int index = Random.Range(0, jumpSounds.Length);
            audioSource.clip = jumpSounds[index];
            audioSource.Play();
        }
    }
}