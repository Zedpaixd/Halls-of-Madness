using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySoundController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip attentionClip;
    [SerializeField]
    private AudioClip attackClip;

    public bool attention = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = Settings.sfxVolume * Settings.masterVolume;
    }

    public void PlayAttention()
    {
        if (audioSource.isPlaying)
        {
            PauseTime();
        }
        else
        {
            if (!attention)
            {
                attention = true;
                audioSource.clip = attentionClip;
                audioSource.Play();
            }
        }
    }

    public void PlayAttack()
    {
        audioSource.clip = attackClip;
        audioSource.Play();
    }
    
    private IEnumerator PauseTime()
    {
        yield return new WaitForSeconds(1);
    }


}
