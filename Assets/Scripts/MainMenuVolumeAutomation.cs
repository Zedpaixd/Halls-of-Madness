using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class MainMenuVolumeAutomation : MonoBehaviour
{
    float caOffset, vgOffset;
    public float caSpeed, caIntensity, caSubtraction;
    public float vgSpeed, vgIntensity, vgVariation;
    Volume volume;
    ChromaticAberration ca;
    Vignette vg;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out ca);
        volume.profile.TryGet(out vg);
    }

    void Update()
    {
        caOffset += Time.deltaTime + caSpeed/1000;
        vgOffset += Time.deltaTime + vgSpeed/100000;
        ca.intensity.Override((Mathf.PerlinNoise(0, caOffset)-caSubtraction)*caIntensity);
        vg.intensity.Override(vgIntensity-vgVariation+2*vgVariation*Mathf.PerlinNoise(0, vgOffset));
    }
}
