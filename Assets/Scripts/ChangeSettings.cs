using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ChangeSettings : MonoBehaviour
{
    public Volume brightnessVolume;
    ColorAdjustments colorAdj;

    public bool isMainScene;
    public float bcNoiseRelativeVol = 0.2f, playerAudioRelativeVol = 0.5f, entityRelativeVol = 1f;
    public AudioSource playerAudioSource;
    AudioSource[] entityAudioSources;
    public AudioSource bcNoise;
    private void Start()
    {
        var entities = GameObject.FindGameObjectsWithTag("Entity");
        entityAudioSources = new AudioSource[entities.Length];
        for (int i = 0; i < entities.Length; i++)
        {
            entityAudioSources[i] = entities[i].GetComponent<AudioSource>();
        }
        brightnessVolume.profile.TryGet(out colorAdj);
        ApplyAllSettings();
    }
    public void ApplyAllSettings()
    {
        if (!isMainScene)
        {
            playerAudioSource.volume = playerAudioRelativeVol * Settings.masterVolume * Settings.sfxVolume;
            foreach (AudioSource audioSource in entityAudioSources)
            {
                audioSource.volume = entityRelativeVol * Settings.masterVolume * Settings.sfxVolume;
            }
        }
        bcNoise.volume = bcNoiseRelativeVol * Settings.masterVolume * Settings.ambienceVolume;
        colorAdj.postExposure.Override(-3 + 6 * Settings.brightness);
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
            foreach (AudioSource audioSource in entityAudioSources)
            {
                audioSource.volume = entityRelativeVol * Settings.masterVolume * Settings.sfxVolume;
            }
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
        colorAdj.postExposure.Override(-3+6*value);
    }
}
