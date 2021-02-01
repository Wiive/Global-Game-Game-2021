using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    [SerializeField] EventSystem mainMenuEventSystem = null;
    public GameObject audioMenu;

    public GameObject gameOverMenu;
    [SerializeField] GameObject firstGameOverButton = null;
    [SerializeField] EventSystem gameOverEventSystem = null;

    public GameObject pauseMenu;
    [SerializeField] EventSystem pauseEventSystem = null;
    public GameObject highscoreMenu;

    [SerializeField] GameObject lastPressedPauseButton = null;
    [SerializeField] GameObject lastPressedMainMenuButton = null;
    [SerializeField] GameObject lastPressedGameOVerButton = null;


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
        GetLastPressedButton();
    }

    public void ShowAudioMenu()
    {
        SetLastPressedButton();
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameOver);

        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        highscoreMenu.SetActive(false);
        gameOverEventSystem.SetSelectedGameObject(null);
        gameOverEventSystem.SetSelectedGameObject(firstGameOverButton);
        GetLastPressedButton();
    }

    public void ShowPauseMenus()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(true);
        highscoreMenu.SetActive(false);
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.IngameMenu);
        GetLastPressedButton();
    }

    public void ShowHighscoreMenu()
    {
        SetLastPressedButton();
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

    private void SetLastPressedButton()
    {
        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.MainMenu)
        {
            lastPressedMainMenuButton = mainMenuEventSystem.currentSelectedGameObject;
        }
        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameOver)
        {
            lastPressedGameOVerButton = gameOverEventSystem.currentSelectedGameObject;
        }
        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.IngameMenu)
        {
            lastPressedPauseButton = pauseEventSystem.currentSelectedGameObject;
        }
    }

    private void GetLastPressedButton()
    {
        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.MainMenu)
        {
            if (lastPressedMainMenuButton == null) return;
            mainMenuEventSystem.firstSelectedGameObject = lastPressedMainMenuButton;
            mainMenuEventSystem.SetSelectedGameObject(lastPressedMainMenuButton);
        }
        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameOver)
        {
            if (lastPressedGameOVerButton == null) return;
            gameOverEventSystem.firstSelectedGameObject = lastPressedGameOVerButton;
            gameOverEventSystem.SetSelectedGameObject(lastPressedGameOVerButton);
        }
        else if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.IngameMenu)
        {
            if (lastPressedPauseButton == null) return;
            pauseEventSystem.firstSelectedGameObject = lastPressedPauseButton;
            pauseEventSystem.SetSelectedGameObject(lastPressedPauseButton);
        }
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
        GetLastPressedButton();
    }
}
