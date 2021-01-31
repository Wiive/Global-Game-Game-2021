using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClick : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip UiClick;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayUiClick()
    {
        audioSource.PlayOneShot(UiClick);
    }
}
