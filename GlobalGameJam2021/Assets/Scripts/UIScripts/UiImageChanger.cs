using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UiImageChanger : MonoBehaviour
{

    public CanvasGroup canvas;
    public float timeBeforeFade;
    public float fadeDuration;
    bool fadeIn = false;
    float time;


    void Update()
    {

        if (time > timeBeforeFade && !fadeIn)
        {
            fadeIn = true;
            time = 0f;
        }

        if (fadeIn)
        {
            canvas.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
        }

        time += Time.deltaTime;
    }
}
