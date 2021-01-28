using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();       
    }

    private void OnEnable()
    {
        EventManager.instance.onScoreUpdate += UpdateScore;
    }

    private void OnDisable()
    {
        EventManager.instance.onScoreUpdate -= UpdateScore;
    }

    private void UpdateScore(int score)
    {      
        scoreText.text = score.ToString();
    }
}
