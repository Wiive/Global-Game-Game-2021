using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip SpawnSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PlaySound(SpawnSound);
        }
    }


    public void PlaySound(AudioClip soundToPlay)
    {
        //if (soundPlayer.isPlaying)
        //{
        //    soundPlayer.Stop();
        //}
        audioSource.clip = soundToPlay;
        audioSource.Play();
    }
}
