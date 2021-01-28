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
        moveController.Move(direction);
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
    
    protected virtual void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Relic")
        {
            Pickup(other.GetComponent<Relic>());
        }
    }
}
