using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MovementController))]
public class Character : MonoBehaviour
{
    protected Animator animator;
    protected MovementController moveController;
    
    [SerializeField] protected float speed = 60f;
    [SerializeField] protected Vector2 direction;
    public bool isAlive = true;

    protected virtual void Awake()
    {
        GetAllComponents();
    }
    
    protected virtual void Start()
    {
    }
    
    protected virtual void Update()
    {
    }
    
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void GetAllComponents()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<MovementController>();
    }

    protected virtual void Move()
    {
        moveController.Move(direction * speed * Time.deltaTime);
    }

    protected virtual void GotKilled()
    {
        Debug.Log($"{name} got Killed!");
        isAlive = false;
    }
    
    protected virtual void Attack()
    {
        Debug.Log($"{name} Attacks!");
    }
    
    protected virtual void Pickup()
    {
        Debug.Log($"{name} Pickups Relic!");
    }
}
