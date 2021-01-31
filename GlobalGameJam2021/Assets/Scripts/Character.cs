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
    private Transform gfxTransform;

    private Vector2Int currentPos = new Vector2Int(0,0);
    public Vector2Int CurrentPos { get { return currentPos; } set { currentPos = value; } }

    [SerializeField] protected Vector2 direction;
    public bool isAlive = true;
    
    private int tileSize = 0;
    public int TileSize { get { return tileSize; } set { tileSize = value; } }

    private BoxCollider2D selfCollider;

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
        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            Move();
        }    
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
        selfCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Init()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("DirectionX", 0);
        animator.SetFloat("DirectionY", -1);
    }

    protected virtual void Move()
    {
        moveController.Move();
    }

    protected virtual void Respawn()
    {
        selfCollider.enabled = true;
        isAlive = true;
    }

    public virtual void GotKilled()
    {
        // Debug.Log($"{name} got Killed!");
        selfCollider.enabled = false;
        isAlive = false;
    }
    
    protected virtual void Attack(Character character)
    {
        // Debug.Log($"{name} Attacks {character.name}!");
        character.GotKilled();
    }
    
    protected virtual void Pickup(Relic relic)
    {
        // Debug.Log($"{name} Pickups Relic \"{relic.name}\"!");
    }

    protected virtual void UpdateAnimations()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }
        
        animator.SetBool("IsMoving", moveController.IsMoving);
    }
    
    protected virtual void UpdateTimers()
    {
    }

    private void FixPixelPosition ()
    {
        int x = Mathf.RoundToInt (transform.position.x);
        int y = Mathf.RoundToInt (transform.position.y);

        gfxTransform.position = new Vector3( x, y, 0);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Relic"))
            Pickup(other.GetComponent<Relic>());
    }
}
