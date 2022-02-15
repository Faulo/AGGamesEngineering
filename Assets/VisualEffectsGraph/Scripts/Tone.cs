using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tone : MonoBehaviour
{
    [SerializeField]
    KeyCode playButton = KeyCode.A;
    [SerializeField, Range(0, 24000)]
    float frequency = 200F;
    [SerializeField]
    string note = "A";

    int time = 0;
    int sampleRate = 48000;
    bool isPlaying = false;

    private void Start() {
        sampleRate = AudioSettings.outputSampleRate;
    }

    private void Update() {
        isPlaying = Input.GetKey(playButton);
    }

    private void OnAudioFilterRead(float[] data, int channels) {
        for (int i = 0; i < data.Length;) {
            time++;
            for (int j=0; j< channels; j++) {
                if (isPlaying) {
                    data[i] += Mathf.Sin((2F * Mathf.PI * time * frequency) / sampleRate);
                }
                i++;
            }
        }
    }
}
