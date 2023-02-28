using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip stepClip;
    [SerializeField]
    private AudioClip startJumpClip;
    [SerializeField]
    private AudioClip landJumpClip;

    public bool isWalking = false;
    public bool mustLand = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ChangeToStep();
    }

    private void Update()
    {
        // Stop step audio if player stop walking
        if (!isWalking && (audioSource.clip == stepClip && audioSource.isPlaying))
        {
            audioSource.Stop();
        }
    }

    public bool CanChangeToStep()
    {
        if ((audioSource.isPlaying && !audioSource.loop) || (audioSource.clip == stepClip && audioSource.isPlaying))
        {
            return false;
        }
        return true;
    }
    public void ChangeToStep()
    {
        audioSource.clip = stepClip;
        audioSource.loop = true;
    }
    
    public void PlayStartJump()
    {

        audioSource.clip = startJumpClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayLandJump()
    {
        audioSource.clip = landJumpClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
