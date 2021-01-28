using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    // TODO REMOVE! TEMP!!!
    private void Start()
    {
        SetRandomPosition();
    }

    public void ReturnToStartPosition()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        // TODO REMOVE! TEMP!!!
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetRandomPosition();
        }
    }

    // TODO REMOVE! TEMP!!!
    private void SetRandomPosition()
    {
        if ((Vector2)transform.position == startPosition)
            return;
        
        int tileSize = 16;
        int screenGridWidth = Mathf.FloorToInt(320f / tileSize);
        int screenGridHeight = Mathf.FloorToInt(180f / tileSize);

        transform.position = new Vector2(UnityEngine.Random.Range(0, screenGridWidth) * tileSize, 
                                                 UnityEngine.Random.Range(0, screenGridHeight) * tileSize);
    }
}
