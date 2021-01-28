using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private Rigidbody2D body;
    private Collider2D myCollider;

    private void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        body = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        body.velocity = velocity;
    }
}
