using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9)]
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public enum GameState
    {
        MainMenu,
        IngameMenu,
        GameLoop,
        GameOver,
        Victory
    }

    private GameState currentGameState;
    private GameState previousGameState;

    public GameState CurrentGameState => currentGameState;
    public GameState PreviousGameState => previousGameState;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    
    public Action<GameStateManager.GameState> onChangeGameState;
    public void ChangeGameState(GameState newGameState)
    {
        if (newGameState == currentGameState)
            return;

        
        if (newGameState == GameState.MainMenu)
        {
            previousGameState = GameState.MainMenu;
        }
        
        if (newGameState == GameState.IngameMenu)
        {
            previousGameState = GameState.IngameMenu;
        }
        
        if (newGameState == GameState.GameLoop)
        {
            previousGameState = GameState.GameLoop;
        }
        
        if (newGameState == GameState.GameOver)
        {
            previousGameState = GameState.GameOver;
        }
        
        if (newGameState == GameState.Victory)
        {
            previousGameState = GameState.Victory;
        }

        currentGameState = newGameState;
        
        onChangeGameState?.Invoke(newGameState);
    }
}
