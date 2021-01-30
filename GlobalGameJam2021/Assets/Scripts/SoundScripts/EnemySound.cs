using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySound : MonoBehaviour
{

    AudioSource audioSource;
    public bool test;

    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;
    public AudioClip spawnSound;
    public AudioClip relicPickup;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (test)
        {
            SoundTest();

        }
    }

    private void SoundTest()
    {
        if (Input.GetButtonDown("Jump"))
            PlayPickUpSound();
    }

    

    void PlaySound(AudioClip soundToPlay)
    {
        audioSource.clip = soundToPlay;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    void PlayRandomSoundFromArray(AudioClip[] soundsToPlay)
    {
        audioSource.clip = soundsToPlay[Random.Range(0 , soundsToPlay.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.2f);
        audioSource.Play();
    }

    public void PlayAttackSound()
    {
        PlayRandomSoundFromArray(attackSounds);
    }

    public void PlayPickUpSound()
    {
        PlaySound(relicPickup);
    }

    public void PlayDeathSound()
    {
        PlayRandomSoundFromArray(deathSounds);

    }

    public void PlaySpawnSound()
    {
        audioSource.PlayOneShot(spawnSound);
    }


}
