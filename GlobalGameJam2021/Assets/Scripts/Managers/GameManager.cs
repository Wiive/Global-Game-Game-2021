using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int gameScore;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public Action<int> onScoreUpdate;
    public void AddToScore(int amount)
    {
        gameScore += amount;
        onScoreUpdate?.Invoke(gameScore);
    }

    public int GetCurrentScore()
    {
        return gameScore;
    }
}
