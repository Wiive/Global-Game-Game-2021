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
        Debug.Log(grid.Count);

        foreach (WayPoint waypoint in waypoints)
        {
            waypoint.isExplored = false;
            AddToGrid(waypoint);
        }
    }
    private void AddToGrid(WayPoint waypoint)
    {
        waypoint.isExplored = false;
        var gridPos = waypoint.GridPos;
        if (grid.ContainsKey(gridPos))
        {
            Debug.LogWarning("Skipping due to overlapping waypoint at " + waypoint.name);
        }
        else
        {
            waypoint.addedToGrid = true;
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
    private void SearchNearby()
    {
        if (!searchForPath) { return; }
        SearchDirections();
    }
    private void EndNodeFound()
    {
        if (searchCenter == endWaypoint)
        {
            searchForPath = false;
            CreatePath();
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
