using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject audioMenu;

   public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        audioMenu.SetActive(false);
    }

    public void ShowAdioMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
    }
}
