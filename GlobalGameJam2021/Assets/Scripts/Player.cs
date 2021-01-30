using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool moveConstant = false;
    Dictionary<Vector2Int, MazeNode> grid = new Dictionary<Vector2Int, MazeNode>();
    PlayerSound playerSound;

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

        if (moveController.IsMoving) return;

        // TODO: REMOVE LATER (DEBUG ONLY)
        DebugInput();
        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));       
        if (moveConstant)
        {
            if ((input.x != 0 && input.y == 0 || input.x == 0 && input.y != 0) && input != Vector2.zero)
                direction = input;
        }
        else
        {
            if (input.x != 0f && input.y != 0f)
                direction = Vector2.zero;
            else
                direction = input;
        }
        
        if (!moveController.IsMoving && direction != Vector2.zero && IsValidInput())
        {
            CurrentPos += new Vector2Int((int)direction.x, (int)direction.y);
            moveController.SetTargetPosition(direction);
        }
    }
    
    bool IsValidInput()
    {
        Vector2Int inputTry = new Vector2Int((int)direction.x, (int)direction.y) + CurrentPos;
        if (grid.ContainsKey(inputTry) && !grid[inputTry].isWall )
            return true;
        // Debug.Log("non valid movement");
        return false;
    }

    protected override void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
        relic.ReturnToStartPosition();
    }
   
    protected override void Attack(Character character)
    {
        base.Attack(character);
    }
    
    public override void GotKilled()
    {
        base.GotKilled();
    }

    protected override void UpdateTimers()
    {
        base.UpdateTimers();
    }   
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Enemy"))
        {           
            Attack(other.GetComponent<Enemy>());
            playerSound.PlayAttackSound();
        }
    }

    // TODO REMOVE LATER (DEBUG ONLY)
    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
            moveConstant = !moveConstant;
    }
}
