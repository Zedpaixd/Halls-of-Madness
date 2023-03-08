using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySoundController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] passiveClips = new AudioClip[4];
    [SerializeField]
    private AudioClip[] angryClips = new AudioClip[4];
    [SerializeField]
    private AudioClip attackClip;

    public bool angry = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = Settings.sfxVolume * Settings.masterVolume * 1.5f;
        Passive();
    }

    public void Angry()
    {
        angry = true;
        CancelInvoke();
        InvokeRepeating("PlayAngrySound", 0f, 2.5f);
    }
    void PlayAngrySound()
    {
        audioSource.clip = angryClips[Random.Range(0, 4)];
        audioSource.Play();
    }
    public void Passive()
    {
        angry = false;
        CancelInvoke();
        InvokeRepeating("PlayPassiveSound", 0f, 6f);
    }
    void PlayPassiveSound()
    {
        audioSource.clip = passiveClips[Random.Range(0, 4)];
        audioSource.Play();
    }

    public void PlayAttack()
    {
        audioSource.clip = attackClip;
        audioSource.Play();
    }
}
