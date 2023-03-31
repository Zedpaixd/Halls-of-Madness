using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ChangeSettings : MonoBehaviour
{
    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    public bool isMainScene;
    public float bcNoiseRelativeVol = 0.4f, playerAudioRelativeVol = 1f;
    AutoExposure exposure;
    public AudioSource playerAudioSource;
    public AudioSource bcNoise;
    private void Start()
    {
        brightness.TryGetSettings(out exposure);
        ApplyAllSettings();
    }
    public void ApplyAllSettings()
    {
        if (!isMainScene)
        {
            playerAudioSource.volume = playerAudioRelativeVol * Settings.masterVolume * Settings.sfxVolume;
        }
        bcNoise.volume = bcNoiseRelativeVol * Settings.masterVolume * Settings.ambienceVolume;
        exposure.keyValue.value = 0.1f * Mathf.Pow(100, Settings.brightness);
    }
    public void SetMasterVolume(float value)
    {
        Settings.masterVolume = value;
        if (!isMainScene)
        {
            playerAudioSource.volume = playerAudioRelativeVol * value * Settings.sfxVolume;
        }
        bcNoise.volume = bcNoiseRelativeVol * value * Settings.ambienceVolume;
    }
    
    public void SetSfxVolume(float value)
    {
        Settings.sfxVolume = value;
        if (!isMainScene)
        {
            playerAudioSource.volume = playerAudioRelativeVol * value * Settings.masterVolume;
        }
    }
    public void SetAmbianceVolume(float value)
    {
        Settings.ambienceVolume = value;
        bcNoise.volume = bcNoiseRelativeVol * value * Settings.masterVolume;
    }
    public void SetBrightness(float value)
    {
        Settings.brightness = value;
        exposure.keyValue.value = 0.1f*Mathf.Pow(100,value);
    }
}
