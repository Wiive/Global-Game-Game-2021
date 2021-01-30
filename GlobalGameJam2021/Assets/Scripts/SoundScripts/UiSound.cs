using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiSound : MonoBehaviour
{
    public static UiSound instance;

    AudioSource soundPlayer;
    public AudioClip enemyRelicPickUp;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

    }

    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.instance.onPickedUpItem += OnRelicPickUp;
    }

    private void OnDisable()
    {
        EventManager.instance.onPickedUpItem -= OnRelicPickUp;
    }

    void OnRelicPickUp(PickerItemWrapper pickUpEvent)
    {

        if (pickUpEvent.picker.CompareTag("Enemy"))
        {
            PlayPickupWarningSound();

        }
    }

    public void PlayPickupWarningSound()
    {
        soundPlayer.PlayOneShot(enemyRelicPickUp);
    }








}
