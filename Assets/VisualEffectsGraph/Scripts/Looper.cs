using UnityEngine;

namespace AGGE.VisualEffectsGraph {
    public class Looper : MonoBehaviour {
        enum Tempo {
            full = 1,
            half = 2,
            quarter = 4,
            eigth = 8,
            sixteenth = 16
        }

        [SerializeField]
        AudioSource audioSource = default;
        [SerializeField]
        float bpm = 120;
        [SerializeField]
        Tempo tempo = Tempo.quarter;
        [SerializeField]
        AudioClip clip = default;
        [SerializeField]
        Tempo offset = Tempo.full;

        double nextBeat = 0;

        protected void Start() {
            nextBeat = AudioSettings.dspTime + CalculateBeatTime(offset);
            audioSource.Play();
        }

        double CalculateBeatTime(Tempo tempo) {
            double fullBeatTime = 60F / bpm;
            return fullBeatTime / (int)tempo;
        }

        // Update is called once per frame
        protected void Update() {
            if (AudioSettings.dspTime > nextBeat) {
                audioSource.PlayOneShot(clip);
                nextBeat += CalculateBeatTime(tempo);
            }
        }
    }
}