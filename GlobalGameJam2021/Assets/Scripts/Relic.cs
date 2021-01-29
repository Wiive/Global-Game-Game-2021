using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    public RelicData data;

    SpriteRenderer spriteRenderer;
    MazeNode spawnPoint;
    public MazeNode SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }

    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // TODO REMOVE! TEMP!!!
    private void Start()
    {       
        //SetRandomPosition();
    }

    public void ReturnToStartPosition()
    {
        transform.position = spawnPoint.transform.position;
        spawnPoint.hasRelic = true;
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
        if ((Vector2)transform.position != startPosition)
            return;
        
        int tileSize = 16;
        int screenGridWidth = Mathf.FloorToInt(320f / tileSize);
        int screenGridHeight = Mathf.FloorToInt(180f / tileSize);

        float x = (UnityEngine.Random.Range(0, screenGridWidth) * tileSize);
        float y = (UnityEngine.Random.Range(0, screenGridWidth) * tileSize);

        transform.position = new Vector2(x, y);
    }

    public void SetData(RelicData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
    }
}
