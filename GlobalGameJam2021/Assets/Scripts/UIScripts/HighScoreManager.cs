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
        LoadScore();
    }

    void LoadScore()
    {
        score1 = PlayerPrefs.GetInt("Score-1", 500);
        score2 = PlayerPrefs.GetInt("Score-2", 400);
        score3 = PlayerPrefs.GetInt("Score-3", 300);
        score4 = PlayerPrefs.GetInt("Score-4", 200);
    }

    public void UpdateHighscoreUI()
    {
        LoadScore();
        Debug.Log("Trying to update Highscore UI");
        number1.text = score1.ToString();
        number2.text = score2.ToString();
        number3.text = score3.ToString();
        number4.text = score4.ToString();
    }

    public void SetHighScore()
    {
        if(IsNewScoreHighScore())
        {
            Debug.Log("Truing to set new hihscore");
            Debug.Log(GameManager.instance.GetCurrentScore());
            valueList.Add(score1);
            valueList.Add(score2);
            valueList.Add(score3);
            valueList.Add(score4);
            valueList.Add(GameManager.instance.GetCurrentScore());

            valueList.Sort();

            for (int i = 0; i < valueList.Count; i++)
            {              
               Debug.Log(i);
               Debug.Log(PlayerPrefs.GetInt("Score-" + i));
               PlayerPrefs.SetInt("score-" + (valueList.Count - i), valueList[i]);
            }
        }
    }

    bool IsNewScoreHighScore()
    {
        if (GameManager.instance.GetCurrentScore() >= PlayerPrefs.GetInt("Score-4"))
        {
            Debug.Log("Gz new Hi-Score! " + GameManager.instance.GetCurrentScore() + " > " + PlayerPrefs.GetInt("Score-4"));
            return true;
        }      
        return false;
    }
}
