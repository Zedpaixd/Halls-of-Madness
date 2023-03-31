using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplySettingsToSliders : MonoBehaviour
{
    void Start()
    {
        var soundSliders = transform.GetChild(1).GetComponentsInChildren<Slider>();
        soundSliders[0].value = Settings.masterVolume;
        soundSliders[1].value = Settings.ambienceVolume;
        soundSliders[2].value = Settings.sfxVolume;
        transform.GetChild(2).GetComponentInChildren<Slider>().value = Settings.brightness;
    }
}
