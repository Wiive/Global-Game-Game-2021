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

    private void Start()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameLoop);
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
    
    public void AddToScore(int amount)
    {
        gameScore += amount;
        EventManager.instance.BroadcastOnScoreUpdate(gameScore);
    }

    public int GetCurrentScore()
    {
        return gameScore;
    }
    
    public void PickedUpObject(GameObject picker, GameObject pickedObject)
    {
        PickerItemWrapper newPickerObject = new PickerItemWrapper();

        newPickerObject.picker = picker;
        newPickerObject.pickedItem = pickedObject;

        EventManager.instance.BroadcastOnPickedUpItem(newPickerObject);
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
