using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    public RelicData data;

    [SerializeField] Enemy carrier;

    SpriteRenderer spriteRenderer;
    [SerializeField] MazeNode spawnPoint;
    public MazeNode SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }

    BoxCollider2D boxCollider2D;

    private Vector2 startPosition;


    private void Awake()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (carrier != null)
            transform.position = carrier.transform.position;
        else
        {
            ReturnToStartPosition();
        }
    }

    public void ReturnToStartPosition()
    {
        transform.position = spawnPoint.transform.position;
        carrier = null;
        spawnPoint.hasRelic = true;
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
    }

    public void SetData(RelicData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
    }

    public void GetPickedUp(Enemy carrier)
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        this.carrier = carrier;
    }
}
