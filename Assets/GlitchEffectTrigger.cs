using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchEffectTrigger : MonoBehaviour
{
    public GameObject glitchEffect;
    public bool oneTime;
    bool used;
    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        glitchEffect.SetActive(true);
        if (!oneTime) return;
        used = true;
        Invoke("SetEffectInactive", 1f);
    }
    private void OnTriggerExit(Collider other)
    {
        if (oneTime) return;
        glitchEffect.SetActive(false);
    }
    void SetEffectInactive()
    {
        glitchEffect.SetActive(false);
        gameObject.SetActive(false);
    }
}
