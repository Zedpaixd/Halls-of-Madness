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
    private void Start()
    {
        brightness.TryGetSettings(out exposure);
    }
    public void SetMasterVolume(float value)
    {
        Settings.masterVolume = value;
        playerAudioSource.volume = value*Settings.sfxVolume;
    }
    public void SetSfxVolume(float value)
    {
        Settings.sfxVolume = value;
        playerAudioSource.volume = value*Settings.masterVolume;
    }
    public void SetAmbianceVolume(float value)
    {
        Settings.ambianceVolume = value;
    }
    public void SetBrightness(float value)
    {
        Settings.brightness = value;
        exposure.keyValue.value = 0.1f*Mathf.Pow(100,value);
    }
}
