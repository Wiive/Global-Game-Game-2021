using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private bool followPlayer;
    
    private void OnEnable()
    {
        GameStateManager.instance.onChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        GameStateManager.instance.onChangeGameState -= OnChangeGameState;
    }

    private void Update()
    {
        if(followPlayer)
        {
            transform.position = player.position;
        }
    }

    void OnChangeGameState(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.GameLoop)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            
            followPlayer = true;
        }
        else
        {
            followPlayer = false;
        }
    }
}
