using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Audio Data Binder")]
[VFXBinder("Audio/Audio Data to AttributeMap")]
public class VFXAudioPropertyBinder : VFXBinderBase {

    public enum AudioSourceMode {
        AudioSource,
        AudioListener
    }

    public string countProperty { get { return (string)m_CountProperty; } set { m_CountProperty = value; } }
    [VFXPropertyBinding("System.UInt32"), SerializeField]
    protected ExposedProperty m_CountProperty = "Count";

    public string audioOutputProperty { get { return (string)m_audioOutputProperty; } set { m_audioOutputProperty = value; } }
    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    protected ExposedProperty m_audioOutputProperty = "AudioTexture";

    public uint samples = 64;
    public AudioSourceMode mode = AudioSourceMode.AudioSource;
    public AudioSource audioSource = null;

    private Texture2D textureCache;
    private float[] audioCache;
    private Color[] colorCache;

    public override bool IsValid(VisualEffect component) {
        bool modeValid = mode == AudioSourceMode.AudioSource ? audioSource != null : true;
        bool texture = component.HasTexture(audioOutputProperty);
        bool count = component.HasUInt(countProperty);

        return modeValid && texture && count;
    }

    public override void UpdateBinding(VisualEffect component) {
        UpdateTexture();
        component.SetTexture(audioOutputProperty, textureCache);
        component.SetUInt(countProperty, samples);
    }

    private void UpdateTexture() {
        if (textureCache == null || textureCache.width != samples) {
            textureCache = new Texture2D((int)samples, 1, TextureFormat.RFloat, false);
            audioCache = new float[samples];
            colorCache = new Color[samples];
        }

        if (mode == AudioSourceMode.AudioListener)
            AudioListener.GetOutputData(audioCache, 0);
        else if (mode == AudioSourceMode.AudioSource)
            audioSource.GetOutputData(audioCache, 0);
        else
            throw new NotImplementedException();

        for (int i = 0; i < samples; i++) {
            float value = Mathf.InverseLerp(-1, 1, audioCache[i]);
            colorCache[i] = new Color(value, 0, 0, 0);
        }

        textureCache.SetPixels(colorCache);
        textureCache.name = "AudioData" + samples;
        textureCache.Apply();
    }
}
