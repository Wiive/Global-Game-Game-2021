using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private Rigidbody2D body;
    private Collider2D myCollider;
    
    [SerializeField] protected float speed = 60f;
    private Vector2 targetPosition;
    private bool moveIsDone;
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

    public void Move(Vector2 direction)
    {
        // body.velocity = velocity;
        
        if (moveIsDone)
        {
            // targetPosition = transform.position + (direction * TileSize);
        }
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
