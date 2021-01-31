using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuSoundController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioMixer mixer;
    public Slider soundSlider;
    public Slider musicSlider;
    public TextMeshProUGUI soundText;
    public TextMeshProUGUI musicText;
    public AudioClip UiClick;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20;
            mixer.SetFloat("MusicVolume", volume);
        }
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            float volume = Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume")) * 20;
            mixer.SetFloat("SoundVolume", volume);

        }
    }

    public void SetMusicVolume()
    {
        UpdateText(musicText, "Music: " + (int)(musicSlider.value * 100) + "%");
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        float volume = Mathf.Log10(musicSlider.value) * 20;
        mixer.SetFloat("MusicVolume", volume);
    }

    public void SetSoundVolume()
    {
        UpdateText(soundText, "Sound: " + (int)(soundSlider.value * 100) + "%");
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        float volume = Mathf.Log10(soundSlider.value) * 20;
        mixer.SetFloat("SoundVolume", volume);
    }

    public void PlayUiClick()
    {
        audioSource.PlayOneShot(UiClick);
    }


    public void UpdateText(TextMeshProUGUI textMesh, string updatedText)
    {
        textMesh.text = updatedText;
    }


}
