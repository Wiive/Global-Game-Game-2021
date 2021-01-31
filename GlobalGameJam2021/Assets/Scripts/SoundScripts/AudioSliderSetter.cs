using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioSliderSetter : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;
    private void OnEnable()
    {
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.7f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.7f);

    }

}
