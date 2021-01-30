using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{

    public bool test = false;
    private AudioSource audioSource;
    public AudioClip AttackSound;
    public AudioClip SpawnSound;
    public AudioClip Ping;
    

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

    void SoundTest()
    {
        if (Input.GetButtonDown("Jump"))
        {
            audioSource.PlayOneShot(Ping);           
        }
    }

    void PlaySound(AudioClip soundToPlay)
    {

        audioSource.clip = soundToPlay;
        audioSource.Play();
    }

    public void PlayAttackSound()
    {
        PlaySound(AttackSound);
    }
    public void PlaySpawnSound()
    {       
        audioSource.PlayOneShot(SpawnSound);
    }





}
