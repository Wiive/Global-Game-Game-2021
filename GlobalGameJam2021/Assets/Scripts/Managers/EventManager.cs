using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    
    // SCORE UPDATE
    public Action<int> onScoreUpdate;
    public void BroadcastOnScoreUpdate(int amount)
    {
        onScoreUpdate?.Invoke(amount);
    }
    
    // CHANGE GAMESTATE
    public Action<GameStateManager.GameState> onChangeGameState;
    public void BroadcastOnChangeGameState(GameStateManager.GameState newGameState)
    {
        onChangeGameState?.Invoke(newGameState);
    }

    // PICKED UP ITEM
    public Action<PickerItemWrapper> onPickedUpItem;
    public void BroadcastOnPickedUpItem(PickerItemWrapper newPickerObject)
    {
        onPickedUpItem?.Invoke(newPickerObject);
    }
}
