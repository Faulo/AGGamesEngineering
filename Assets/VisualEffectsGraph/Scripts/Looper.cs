using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour
{
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

    private void Start() {
        nextBeat = AudioSettings.dspTime + CalculateBeatTime(offset);
        audioSource.Play();
    }

    private double CalculateBeatTime(Tempo tempo) {
        double fullBeatTime = 60F / bpm;
        return fullBeatTime / (int)tempo;
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioSettings.dspTime > nextBeat) {
            audioSource.PlayOneShot(clip);
            nextBeat += CalculateBeatTime(tempo);
        }
    }
}
