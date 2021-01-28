using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Transform player;
    [SerializeField] private BoxCollider2D roomBorder; 
    private bool followPlayer;
    private float _z;
    [SerializeField] private Vector2 offset;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _z = transform.position.z;
    }

    private void OnEnable()
    {
        EventManager.instance.onChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        EventManager.instance.onChangeGameState -= OnChangeGameState;
    }

    private void LateUpdate()
    {
        if (followPlayer)
            UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (!followPlayer || roomBorder == null)
            return;

        Vector2 targetPos = player.position;
        float x = Mathf.Round(targetPos.x + offset.x);
        float y = Mathf.Round(targetPos.y + offset.y);

        Bounds bounds = roomBorder.bounds;

        float height = _camera.orthographicSize * 2f;
        float width = (height / 9f) * 16f;

        float xDiff = (bounds.size.x - width) / 2f;
        float yDiff = (bounds.size.y - height) / 2f;

        float xMin = bounds.center.x - xDiff;
        float xMax = bounds.center.x + xDiff;
        float yMin = bounds.center.y - yDiff;
        float yMax = bounds.center.y + yDiff;

        transform.position = new Vector3(Mathf.Round( Mathf.Clamp(x, xMin, xMax) ),
                                         Mathf.Round( Mathf.Clamp(y, yMin, yMax) ), _z);
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
