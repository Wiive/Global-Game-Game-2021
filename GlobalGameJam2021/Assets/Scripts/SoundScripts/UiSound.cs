using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiSound : MonoBehaviour
{
    public static UiSound instance;

    AudioSource soundPlayer;

    void Start()
    {
        if (instance != null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        soundPlayer = GetComponent<AudioSource>();
    }






}
