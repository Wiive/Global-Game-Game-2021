using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject audioMenu;

    public GameObject gameOverMenu;
    [SerializeField] GameObject firstGameOverButton = null;
    [SerializeField] EventSystem gameOverEventSystem = null;

    public GameObject pauseMenu;
    public GameObject highscoreMenu;


    private void Update()
    {
        if (Input.GetButtonDown("Pause") && GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            ShowPauseMenus();
        }
        else if (Input.GetButtonDown("Pause") && GameStateManager.instance.CurrentGameState == GameStateManager.GameState.IngameMenu)
        {
            CloseAllMenus();
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

    public void ShowAudioMenu()
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
        gameOverEventSystem.SetSelectedGameObject(null);
        gameOverEventSystem.SetSelectedGameObject(firstGameOverButton);
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

    public void HighscoreBack()
    {
        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.MainMenu)
        {
            ShowMainMenu();
        }

        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameOver)
        {
            ShowGameOverMenu();
        }

        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.IngameMenu)
        {
            ShowPauseMenus();
        }

    }
}
