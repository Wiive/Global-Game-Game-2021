using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] WayPoint startWaypoint, endWaypoint;
    [SerializeField] Vector2Int gridSize = new Vector2Int(0,0); // make this get set after the grid creator.
    public Vector2Int GridSize { get { return gridSize; } set { gridSize = value; } }
    Dictionary<Vector2Int, WayPoint> grid = new Dictionary<Vector2Int, WayPoint>();
    Queue<WayPoint> waypointQueue = new Queue<WayPoint>();
    List<WayPoint> path = new List<WayPoint>();

    WayPoint startSearchPoint;
    Queue<WayPoint> searchPoints = new Queue<WayPoint>();


    bool searchForPath = false;

    WayPoint searchCenter;
    WayPoint[] waypoints;

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
        LoadBlocks();
    }
    private void LoadBlocks()
    {
        grid = new Dictionary<Vector2Int, WayPoint>();
        waypoints = FindObjectsOfType<WayPoint>();

        foreach (WayPoint waypoint in waypoints)
        {
            AddToGrid(waypoint);
        }
    }
    private void AddToGrid(WayPoint waypoint)
    {
        waypoint.isExplored = false;
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


    public List<WayPoint> SearchForPath(Vector2Int startPoint, Vector2Int endPoint)
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
        foreach (var waypoint in waypoints)
        {
            waypoint.isExplored = false;
            waypoint.exploredFrom = null;
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
        WayPoint nearbyWaypoint = grid[nearbyCoordinates];
        if (nearbyWaypoint.isExplored || waypointQueue.Contains(nearbyWaypoint) || nearbyWaypoint.isBlocked) { return; }
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
        WayPoint previous = endWaypoint.exploredFrom;
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
                Debug.Log("Found Relic"); // remove
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
                Debug.Log("Found Exit"); // remove
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
        WayPoint nearbyWaypoint = grid[nearbyCoordinates];
        if (searchPoints.Contains(nearbyWaypoint) || nearbyWaypoint.isExplored) { return; }
        else
        {
            searchPoints.Enqueue(nearbyWaypoint);
        }
    }



    public WayPoint GetWayPoint(Vector2Int positionKey)
    {
        try
        {
            WayPoint wayPoint = grid[positionKey];
            return wayPoint;
        }
        catch
        {
            Debug.Log("Returning null");
            return null;
        }
    }

}
