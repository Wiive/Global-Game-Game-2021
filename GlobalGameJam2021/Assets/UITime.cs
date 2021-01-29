using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITime : MonoBehaviour
{
    TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        float time;
        time = GameManager.instance.GetCurrentGameTime();

        scoreText.text = "Time: " + time.ToString("0.00");
    }
}
