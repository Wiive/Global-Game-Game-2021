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

    public void ReturnToStartPosition()
    {
        transform.position = startPosition;
    }
}
