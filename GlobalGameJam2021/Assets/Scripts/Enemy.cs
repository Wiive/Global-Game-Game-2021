using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] List<WayPoint> path = new List<WayPoint>();
    [SerializeField] Vector2Int destination = new Vector2Int(0, 0);
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float WaitTime = 2f;

    [SerializeField] Vector2Int currentPos = new Vector2Int(0,0);

    Vector2Int gridSize = new Vector2Int(0,0);

    bool gettingNewPath = false;

    int pathIndexPosition = 0;
    PathFinder pathFinder;
    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        gridSize = pathFinder.GridSize;
        destination = TryToGetDestination();
        path = pathFinder.SearchForPath(currentPos, destination);
    }

    protected override void Update()
    {
        if(pathIndexPosition <= path.Count -2)
        {
            MoveEnemyAlongPath();
        }
        else if(!gettingNewPath)
        {
            gettingNewPath = true;
            StartCoroutine(HandleNewPath());
        }
    }

    protected override void Pickup(Relic relic)
    {
        Debug.Log($"{name} Pickups Relic!");
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Relic")
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
