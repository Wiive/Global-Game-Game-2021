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

    protected SpriteRenderer spriteRenderer;
    
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
        UpdateFaceDirection();
    }
    
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void GetAllComponents()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<MovementController>();
        spriteRenderer = transform.Find("GFX").Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected virtual void Move()
    {
        moveController.Move();
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

    private void UpdateFaceDirection()
    {
        if (direction == Vector2.zero)
            return;

        if (direction == Vector2.up)
            spriteRenderer.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (direction == Vector2.right)
            spriteRenderer.transform.eulerAngles = new Vector3(0, 0, -90);
        else if (direction == Vector2.down)
            spriteRenderer.transform.eulerAngles = new Vector3(0, 0, -180);
        else
            spriteRenderer.transform.eulerAngles = new Vector3(0, 0, -270);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Relic")
        {
            Pickup(other.GetComponent<Relic>());
        }
    }
}
