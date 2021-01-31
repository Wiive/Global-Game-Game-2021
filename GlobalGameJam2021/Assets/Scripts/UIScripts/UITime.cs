using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITime : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        float time;
        time = GameManager.instance.GetCurrentGameTime();

        // scoreText.text = time.ToString("0.00");
        scoreText.text = $"<mspace=mspace=6>{time.ToString("0.00")}</mspace>";
    }
}
