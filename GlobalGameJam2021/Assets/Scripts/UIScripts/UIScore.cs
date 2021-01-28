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
        GameManager.instance.onScoreUpdate += UpdateScore;
    }

    private void OnDisable()
    {
        GameManager.instance.onScoreUpdate -= UpdateScore;
    }

    private void UpdateScore(int score)
    {      
        scoreText.text = score.ToString();
    }
}
