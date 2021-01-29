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

    public void ReturnToStartPosition()
    {
        transform.position = spawnPoint.transform.position;
        spawnPoint.hasRelic = true;
    }

    public void SetData(RelicData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
    }
}
