using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] MazeNode startWaypoint, endWaypoint;
    Dictionary<Vector2Int, MazeNode> grid = new Dictionary<Vector2Int, MazeNode>();
    Queue<MazeNode> waypointQueue = new Queue<MazeNode>();
    List<MazeNode> path = new List<MazeNode>();

    MazeNode startSearchPoint;
    Queue<MazeNode> searchPoints = new Queue<MazeNode>();


    bool searchForPath = false;

    MazeNode searchCenter;

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    };
    Vector2Int nearbyCoordinates;

    private void Awake()
    {
        grid = FindObjectOfType<MazeCreator>().Grid;
    }

    public List<MazeNode> SearchForPath(Vector2Int startPoint, Vector2Int endPoint)
    {
        startWaypoint = GetWayPoint(startPoint);
        endWaypoint = GetWayPoint(endPoint);

        ResetWaypoints();
        searchForPath = true;
        waypointQueue.Clear();
        waypointQueue.Enqueue(startWaypoint);
        while (waypointQueue.Count > 0 && searchForPath)
        {
            searchCenter = waypointQueue.Dequeue();
            searchCenter.isExplored = true;
            EndNodeFound();
            SearchNearby();
        }
        return path;
    }
    private void ResetWaypoints()
    {
        foreach (var item in grid)
        {
            item.Value.isExplored = false;
            item.Value.exploredFrom = null;
        }
    }
    private void EndNodeFound()
    {
        if (searchCenter == endWaypoint)
        {
            searchForPath = false;
            CreatePath();
        }
    }
    private void SearchNearby()
    {
        if (!searchForPath) { return; }
        SearchDirections();
    }

    private void SearchDirections()
    {
        foreach (var direction in directions)
        {
            nearbyCoordinates = searchCenter.GridPos + direction;
            if (grid.ContainsKey(nearbyCoordinates))
            {
                QueueNewWaypoints(nearbyCoordinates);
            }
        }
    }

    private void QueueNewWaypoints(Vector2Int nearbyCoordinates)
    {
        MazeNode nearbyWaypoint = grid[nearbyCoordinates];
        if (nearbyWaypoint.isExplored || waypointQueue.Contains(nearbyWaypoint) || nearbyWaypoint.isWall) { return; }
        else
        {
            waypointQueue.Enqueue(nearbyWaypoint);
            nearbyWaypoint.exploredFrom = searchCenter;
        }
    }

    private void CreatePath()
    {
        path.Clear();
        path.Add(endWaypoint);
        MazeNode previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
        {
            path.Add(previous);
            previous = previous.exploredFrom;
        }
        path.Add(startWaypoint);
        path.Reverse();
    }


    // refactor this more nicely! this takes care of the searching for relic
    // and exit in the vicinity (range decides how far it looks) 
    public Vector2Int SearchForRelic(int range, Vector2Int startPoint)
    {
        ResetWaypoints();
        startSearchPoint = GetWayPoint(startPoint);
        searchPoints.Clear();
        searchPoints.Enqueue(startSearchPoint);

        while (searchPoints.Count > 0)
        {
            searchCenter = searchPoints.Dequeue();
            searchCenter.isExplored = true;

            if(searchCenter.hasRelic)
            {
                searchCenter.hasRelic = false;
                return searchCenter.GridPos;            
            }
            else
            {
                SearchDirections(range, startPoint);
            }
        }
        return new Vector2Int(0,0);
    }
    public Vector2Int SearchForExit(int range, Vector2Int startPoint)
    {
        ResetWaypoints();
        startSearchPoint = GetWayPoint(startPoint);
        searchPoints.Clear();
        searchPoints.Enqueue(startSearchPoint);

        while (searchPoints.Count > 0)
        {
            searchCenter = searchPoints.Dequeue();
            searchCenter.isExplored = true;

            if (searchCenter.isExit)
            {
                return searchCenter.GridPos;
            }
            else
            {
                SearchDirections(range, startPoint);
            }
        }
        return new Vector2Int(0, 0);
    }
    private void SearchDirections(int range, Vector2Int startPoint)
    {
        foreach (var direction in directions)
        {
            nearbyCoordinates = searchCenter.GridPos + direction;
            if (Mathf.Abs(nearbyCoordinates.x - startPoint.x) > range || 
                Mathf.Abs(nearbyCoordinates.y - startPoint.y) > range)
            {
                return;
            }

            if (grid.ContainsKey(nearbyCoordinates))
            {
                AddSearchPoint(nearbyCoordinates);
            }
        }
    }
    private void AddSearchPoint(Vector2Int nearbyCoordinates)
    {
        MazeNode nearbyWaypoint = grid[nearbyCoordinates];
        if (searchPoints.Contains(nearbyWaypoint) || nearbyWaypoint.isExplored) { return; }
        else
        {
            searchPoints.Enqueue(nearbyWaypoint);
        }
    }


    public MazeNode GetWayPoint(Vector2Int positionKey)
    {
        try
        {
            MazeNode mazeNode = grid[positionKey];
            return mazeNode;
        }
        catch
        {
            Debug.Log("Returning null");
            return null;
        }
    }

}
