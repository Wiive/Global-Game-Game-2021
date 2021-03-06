using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private Rigidbody2D body;
    private Collider2D myCollider;
    
    [SerializeField] protected float speed = 60f;
    private Vector2 targetPosition;
    private bool isMoving;
    public bool IsMoving => isMoving;

    private const int TileSize = 16;
    
    private void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        body = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    public void Move()
    {
        if (!isMoving)
            return;
        
        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, targetPosition) <= 0.1f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    public void SetTargetPosition(Vector2 direction)
    {
        targetPosition = (Vector2)transform.position + direction * TileSize;
        isMoving = true;
    }
    public void SetEnemyTargetPosition(Vector2 destination)
    {
        targetPosition = destination;
        isMoving = true;
    }
}
