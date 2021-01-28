using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] List<WayPoint> path = new List<WayPoint>();
    [SerializeField] Vector2Int destination = new Vector2Int(0, 0);
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float WaitTime = 2f;
    private Flashlight flashlight;

    [SerializeField] Vector2Int currentPos = new Vector2Int(0,0);

    Vector2Int gridSize = new Vector2Int(0,0);

    bool gettingNewPath = false;

    int pathIndexPosition = 0;
    PathFinder pathFinder;
    
    // TODO REMOVE LATER (Debug Only)
    private float randomDirectionTimer;
    [SerializeField] private float randomDirectionTime = 1f;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (pathIndexPosition <= path.Count -2)
        {
            MoveEnemyAlongPath();
        }
        else if (!gettingNewPath)
        {
            gettingNewPath = true;
            StartCoroutine(HandleNewPath());
        }

        // TODO REMOVE LATER (Debug Only)
        // SetNewRandomDirection();
        //
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

        // randomDirectionTimer = randomDirectionTime;
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

    // TODO REMOVE LATER (Debug Only)
    private void SetNewRandomDirection()
    {
        direction.x = UnityEngine.Random.Range(-1, 1);
        direction.y = direction.x == 0 ? UnityEngine.Random.Range(-1, 1) : 0;
        
        // move
    }

    private void UpdateFlashlightDirection()
    {
        flashlight.UpdateDirection(direction);
    }
    
    protected override void UpdateTimers()
    {
        base.UpdateTimers();

        // randomDirectionTimer -= Time.deltaTime;
        // if (randomDirectionTimer <= 0)
        // {
        //     SetNewRandomDirection();
        //     randomDirectionTimer = randomDirectionTime;
        // }
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
        var targetPosition = path[pathIndexPosition + 1].transform.position;
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);
        if (transform.position == targetPosition)
        {
            pathIndexPosition++;
        }
    }

    IEnumerator HandleNewPath()
    {
        yield return new WaitForSeconds(WaitTime);
        GetNewPath();
        gettingNewPath = false;
    }

    private void GetNewPath()
    {
        currentPos = path[pathIndexPosition].GridPos;
        destination = TryToGetDestination();
        Debug.Log("currentPos: " + currentPos);
        Debug.Log("Destination: " + destination);
        pathIndexPosition = 0;
        path = pathFinder.SearchForPath(currentPos, destination);
    }


    private Vector2Int TryToGetDestination()
    {
        WayPoint destination;
        do
        {
            int x = Random.Range(0, gridSize.x);
            int y = Random.Range(0, gridSize.y);
            Vector2Int wayPointKey = new Vector2Int(x,y);
            destination = pathFinder.GetWayPoint(wayPointKey);
        }
        while (destination == null || destination.isBlocked || destination.GridPos == currentPos);
        return destination.GridPos;
    }
}
