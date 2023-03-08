using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChangeSettings : MonoBehaviour
{
    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    AutoExposure exposure;
    public AudioSource playerAudioSource;
    AudioSource bcNoise;
    private void Start()
    {
        brightness.TryGetSettings(out exposure);
        bcNoise = GameObject.Find("Background Noise").GetComponent<AudioSource>();
    }
    public void SetMasterVolume(float value)
    {
        Settings.masterVolume = value;
        playerAudioSource.volume = value * Settings.sfxVolume;
        bcNoise.volume = value * Settings.ambianceVolume;

    }
    public void SetSfxVolume(float value)
    {
        Settings.sfxVolume = value;
        playerAudioSource.volume = value * Settings.masterVolume;
    }
    public void SetAmbianceVolume(float value)
    {
        Settings.ambianceVolume = value;
        bcNoise.volume = value * Settings.masterVolume;
    }
    public void SetBrightness(float value)
    {
        Settings.brightness = value;
        exposure.keyValue.value = 0.1f*Mathf.Pow(100,value);
    }
}
