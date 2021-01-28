using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-8)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int gameScore;
    private float gameTime;
    private bool countTime;

    private void OnEnable()
    {
        GameStateManager.instance.onChangeGameState += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.instance.onChangeGameState -= OnGameStateChange;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (countTime)
        {
            gameTime += Time.deltaTime;
        }
    }

    public float GetCurrentGameTime()
    {
        return gameTime;
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

    public Action<PickerItemWrapper> onPickedUpObject;
    public void PickedUpObject(GameObject picker, GameObject pickedObject)
    {
        PickerItemWrapper newPickerObject = new PickerItemWrapper();

        newPickerObject.picker = picker;
        newPickerObject.pickedItem = pickedObject;

        onPickedUpObject?.Invoke(newPickerObject);
    }
    
    void OnGameStateChange(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.GameLoop)
        {
            countTime = true;
        }
        else
        {
            countTime = false;
        }
    }
}
