using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] List<WayPoint> path = new List<WayPoint>();
    [SerializeField] Vector2Int destination = new Vector2Int(0, 0);
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float WaitTime = 2f;    
    [SerializeField]int range = 4;
    private Flashlight flashlight;

    Vector2Int gridSize = new Vector2Int(0,0);

    bool gettingNewPath = false;
    bool isCarryingRelic = false;

    int pathIndexPosition = 0;
    PathFinder pathFinder;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        if (pathIndexPosition <= path.Count -2)
        {
            MoveEnemyAlongPath();
        }
        else if (!gettingNewPath)
        {
            gettingNewPath = true;
            StartCoroutine(HandleNewPath());
        }

        // UpdateFlashlightDirection();
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();
        
        pathFinder = GetComponent<PathFinder>();
        flashlight = GetComponentInChildren<Flashlight>();
    }

    protected override void Init()
    {
        base.Init();
        
        gridSize = pathFinder.GridSize;
        destination = TryToGetDestination();
        path = pathFinder.SearchForPath(currentPos, destination);
    }

    protected override void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
    }

    protected override void Attack(Character character)
    {
        base.Attack(character);
    }
    
    protected override void GotKilled()
    {
        base.GotKilled();
    }

    private void UpdateFlashlightDirection()
    {
        flashlight.UpdateDirection(direction);
    }
    
    protected override void UpdateTimers()
    {
        base.UpdateTimers();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Relic"))
        {
            Pickup(other.GetComponent<Relic>());

            GameManager.instance.PickedUpObject(this.gameObject, other.gameObject);
        }
    }

    private void MoveEnemyAlongPath()
    {
        if (moveController.IsMoving)
            return;

        currentPos = path[pathIndexPosition].GridPos;
        direction = path[pathIndexPosition + 1].GridPos - currentPos;
        Debug.Log(direction);
        moveController.SetTargetPosition(direction);
        pathIndexPosition++;
    }

    IEnumerator HandleNewPath()
    {
        currentPos = path[pathIndexPosition].GridPos;
        if (pathFinder.GetWayPoint(currentPos).isExit && isCarryingRelic)
        {
            Debug.Log("I Enemy: " + name + " have now exited the labyrinth with a relic");
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(WaitTime);
        GetNewPath();
        gettingNewPath = false;
    }

    private void GetNewPath()
    {
        destination = TryToGetDestination();
        pathIndexPosition = 0;
        path = pathFinder.SearchForPath(currentPos, destination);
    }

    private Vector2Int TryToGetDestination()
    {
        if (!isCarryingRelic)
        {
            destination = pathFinder.SearchForRelic(range, currentPos);
        }
        else
        {
            destination = pathFinder.SearchForExit(range, currentPos);
        }

        WayPoint wayPointDestination = pathFinder.GetWayPoint(destination);

        if (destination.x == 0 && destination.y == 0)
        {
            do
            {
                int minX = Mathf.Clamp(currentPos.x - range, 0, 100);
                int minY = Mathf.Clamp(currentPos.y - range, 0, 100);

                int maxX = Mathf.Clamp(currentPos.x + range, 0, gridSize.x) + 1;
                int maxY = Mathf.Clamp(currentPos.y + range, 0, gridSize.y) + 1;
                int x = Random.Range(minX, maxX);
                int y = Random.Range(minY, maxY);
                Vector2Int wayPointKey = new Vector2Int(x, y);
                wayPointDestination = pathFinder.GetWayPoint(wayPointKey);
            }
            while (wayPointDestination == null || wayPointDestination.isBlocked || wayPointDestination.GridPos == currentPos);
        }
        else
        {
            isCarryingRelic = true;
        }

        return wayPointDestination.GridPos;
    }
}
