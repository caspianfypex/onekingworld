using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip clickSound;
    public AudioClip hornSound;

    public void PlayClickSound()
    {
        soundSource.clip = clickSound;
        soundSource.Play();
    }

    public void PlayHornSound()
    {
        soundSource.PlayOneShot(hornSound);
    }
}
