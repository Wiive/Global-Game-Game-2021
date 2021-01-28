using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySound : MonoBehaviour
{

    AudioSource soundPlayer;
    public AudioClip[] attackSounds;
    public AudioClip spawnSounds;
    public AudioClip relicPickup;


    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();

    }

    void PlaySound(AudioClip soundToPlay)
    {
        if (soundPlayer.isPlaying)
        {
            soundPlayer.Stop();
        }
        soundPlayer.clip = soundToPlay;
        soundPlayer.Play();
    }

    public void PlayAttackSound()
    {
        PlaySound(attackSounds[0]);
    }

    public void PlayPickUpSound()
    {
        PlaySound(relicPickup);
    }



}
