using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool moveConstant = false;
    Dictionary<Vector2Int, MazeNode> grid = new Dictionary<Vector2Int, MazeNode>();
    PlayerSound playerSound;

    [SerializeField] Vector2 nextDirection = new Vector2(0, 0);

    protected override void Awake()
    {
        base.Awake();
        grid = FindObjectOfType<MazeCreator>().Grid;
    }

    protected override void Start()
    {
        base.Start();
        playerSound = GetComponent<PlayerSound>();
    }

    protected override void Update()
    {
        base.Update();

        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameLoop)
        {
            ReadInput();
        }
    }

    private void ReadInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if ((input.x != 0 && input.y == 0 || input.x == 0 && input.y != 0) && input != Vector2.zero)
        {
            nextDirection = input;
        }

        if (!moveController.IsMoving && nextDirection != Vector2.zero && IsValidInput(nextDirection))
        {
            direction = new Vector2(nextDirection.x, nextDirection.y);
            nextDirection = Vector2.zero;
            animator.SetBool("IsMoving", true);
            CurrentPos += new Vector2Int((int)direction.x, (int)direction.y);
            moveController.SetTargetPosition(direction);
        }
        else if (!moveController.IsMoving && direction != Vector2.zero && IsValidInput(direction))
        {
            animator.SetBool("IsMoving", true);
            CurrentPos += new Vector2Int((int)direction.x, (int)direction.y);
            moveController.SetTargetPosition(direction);
        }
        else if (!moveController.IsMoving)
            animator.SetBool("IsMoving", false);
    }
    
    bool IsValidInput(Vector2 wantedDirection)
    {
        Vector2Int inputTry = new Vector2Int((int)wantedDirection.x, (int)wantedDirection.y) + CurrentPos;
        if (grid.ContainsKey(inputTry) && !grid[inputTry].isWall )
            return true;
        return false;
    }
   
    protected override void Attack(Character character)
    {
        base.Attack(character);
    }
    
    public override void GotKilled()
    {
        base.GotKilled();

        animator.SetBool("IsDead", true);

        MenuManager menuManager;
        if ((menuManager = FindObjectOfType<MenuManager>()) != null)
        {
            menuManager.ShowGameOverMenu();
        }
        else
            Debug.LogError("Missing MenuManager Prefab in Scene");
    }
    
    protected override void UpdateAnimations()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }       
    }

    protected override void UpdateTimers()
    {
        base.UpdateTimers();
    }   
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(other.CompareTag("Flashlight"))
        {
            if(other.transform.parent.GetComponent<Enemy>().isAlive)
                GotKilled();
        }

        if (other.CompareTag("Enemy"))
        {           
            Attack(other.GetComponent<Enemy>());
            playerSound.PlayAttackSound();
        }
    }
}
