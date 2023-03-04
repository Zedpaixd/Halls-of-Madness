using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeSettings : MonoBehaviour
{
    public void SetMasterVolume(float value)
    {
        Settings.masterVolume = value;
    }
    public void SetSfxVolume(float value)
    {
        Settings.sfxVolume = value;
    }
    public void SetAmbianceVolume(float value)
    {
        Settings.ambianceVolume = value;
    }
    public void SetBrightness(float value)
    {
        Settings.brightness = value;
    }
}
