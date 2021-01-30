using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject audioMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            GameStateManager.instance.ChangeGameState(GameStateManager.GameState.IngameMenu);
            ShowPauseMenus();
        }
        

    }


    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ShowAdioMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ShowPauseMenus()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void CloseAllMenus()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        GameStateManager.instance.ChangeGameState(GameStateManager.GameState.GameLoop);
    }
}
