using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MovementController))]
public class Character : MonoBehaviour
{
    protected Animator animator;
    protected MovementController moveController;

    protected SpriteRenderer spriteRenderer;
    private Transform gfxTransform;
    
    [SerializeField] protected Vector2 direction;
    public bool isAlive = true;

    protected virtual void Awake()
    {
        GetAllComponents();
    }
    
    protected virtual void Start()
    {
        Init();
    }
    
    protected virtual void Update()
    {
        UpdateAnimations();
    }
    
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void LateUpdate()
    {
        FixPixelPosition();
    }

    protected virtual void GetAllComponents()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<MovementController>();
        gfxTransform = transform.Find("GFX");
        spriteRenderer = gfxTransform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected virtual void Init()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("DirectionX", 0);
        animator.SetFloat("DirectionY", 1);
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

    private void UpdateAnimations()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }
        animator.SetBool("IsMoving", moveController.IsMoving);
    }

    private void FixPixelPosition ()
    {
        int x = Mathf.RoundToInt (transform.position.x);
        int y = Mathf.RoundToInt (transform.position.y);

        gfxTransform.position = new Vector3( x, y, 0);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Relic")
        {
            Pickup(other.GetComponent<Relic>());
        }
    }
}
