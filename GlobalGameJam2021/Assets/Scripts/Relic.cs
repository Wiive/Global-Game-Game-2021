using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Relic : MonoBehaviour
{
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void ReturnToStartPosition()
    {
        transform.position = startPosition;
    }
    
    // TODO REMOVE! TEMP!!!
    private void SetRandomPosition()
    {
        int screenGridWidth = Mathf.FloorToInt(320 / 16f);
        int screenGridHeight = Mathf.FloorToInt(180 / 16f);

        // transform.position = new Vector2(Random.Range(0, screenGridWidth), Random.Range(0, screenGridHeight));
    }
}
