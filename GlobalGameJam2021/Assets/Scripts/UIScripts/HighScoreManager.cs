using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    List<int> valueList = new List<int>();
    
    public TMP_Text number1;
    public TMP_Text number2;
    public TMP_Text number3;
    public TMP_Text number4;

    private int score1;
    private int score2;
    private int score3;
    private int score4;

    private void Start()
    {
        SetScore();
    }

    void SetScore()
    {
        score1 = PlayerPrefs.GetInt("Score-1", 13000);
        score2 = PlayerPrefs.GetInt("Score-2", 9000);
        score3 = PlayerPrefs.GetInt("Score-3", 5000);
        score4 = PlayerPrefs.GetInt("Score-4", 1500);
    }

    public void UpdateHighscoreUI()
    {
        number1.text = score1.ToString();
        number2.text = score2.ToString();
        number3.text = score3.ToString();
        number4.text = score4.ToString();
    }

    public void SetHighScore()
    {
        if(IsNewScoreHighScore())
        {
            valueList.Add(score1);
            valueList.Add(score2);
            valueList.Add(score3);
            valueList.Add(score4);
            valueList.Add(GameManager.instance.GetCurrentScore());

            valueList.Sort();

            for (int i = 0; i < valueList.Count; i++)
            {
                PlayerPrefs.SetInt("score-" + (valueList.Count - i).ToString(), valueList[i]);
            }
        }
    }

    bool IsNewScoreHighScore()
    {
        if (GameManager.instance.GetCurrentScore() >= PlayerPrefs.GetInt("Score-4"))
        {
            return true;
        }      
        return false;
    }
}
