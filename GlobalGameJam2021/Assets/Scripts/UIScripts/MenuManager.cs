using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject audioMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public GameObject highscoreMenu;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop 
            || Input.GetKeyDown(KeyCode.P) && GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            ShowPauseMenus();
        }       
    }


    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.MainMenu);
    }

    public void ShowAdioMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameOver);

        HighScoreManager scoreManager = GetComponent<HighScoreManager>();
        scoreManager.SetHighScore();

        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
    }

    public void ShowPauseMenus()
    {            
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(true);
        highscoreMenu.SetActive(false);
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.IngameMenu);
    }

    public void ShowHighscoreMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(true);

        HighScoreManager scoreManager = GetComponent<HighScoreManager>();
        scoreManager.UpdateHighscoreUI();
    }

    public void CloseAllMenus()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameLoop);
    }
}
