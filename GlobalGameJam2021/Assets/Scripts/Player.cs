using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private bool moveConstant = false;
    Dictionary<Vector2Int, WayPoint> grid = new Dictionary<Vector2Int, WayPoint>();

    protected override void Awake()
    {
        base.Awake();
        
        LoadBlocks();
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
        
        if (direction != Vector2.zero && IsValidInput())
        {
            currentPos += new Vector2Int((int)direction.x,(int)direction.y);
            moveController.SetTargetPosition(direction);
        }
    }
    
    bool IsValidInput()
    {
        Vector2Int inputTry = new Vector2Int((int) direction.x, (int) direction.y) + currentPos;
        if (grid.ContainsKey(inputTry) && !grid[inputTry].isBlocked )
            return true;
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
    
    protected override void GotKilled()
    {
        base.GotKilled();
    }

    protected override void UpdateTimers()
    {
        base.UpdateTimers();
    }
    
    private void LoadBlocks()
    {
        grid = new Dictionary<Vector2Int, WayPoint>();
        WayPoint[] waypoints = FindObjectsOfType<WayPoint>();

        foreach (WayPoint waypoint in waypoints)
        {
            AddToGrid(waypoint);
        }
    }
    private void AddToGrid(WayPoint waypoint)
    {
        var gridPos = waypoint.GridPos;
        if (grid.ContainsKey(gridPos))
        {
            return;
        }
        else
        {
            grid.Add(gridPos, waypoint);
        }
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        if (other.CompareTag("Enemy"))
            Attack(other.GetComponent<Enemy>());
    }

    // TODO REMOVE LATER (DEBUG ONLY)
    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
            moveConstant = !moveConstant;
    }


}
