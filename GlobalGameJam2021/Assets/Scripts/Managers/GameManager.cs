using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickerObject
{
    public GameObject picker;
    public GameObject pickedItem;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int gameScore;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
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

    public Action<PickerObject> onPickedUpObject;
    public void PickedUpObject(GameObject picker, GameObject pickedObject)
    {
        PickerObject newPickerObject = new PickerObject();

        newPickerObject.picker = picker;
        newPickerObject.pickedItem = pickedObject;

        onPickedUpObject?.Invoke(newPickerObject);
    }
}
