using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public bool isExplored = false;
    public WayPoint exploredFrom;
    public bool isBlocked = false;
    int size = 10;

    public int GetGridSize()
    {
        return size;
    }
    public Vector2Int GetGridPos()
    {
        int xPos = Mathf.RoundToInt(transform.position.x / size);
        int zPos = Mathf.RoundToInt(transform.position.z / size);
        return new Vector2Int(xPos, zPos);
    }
}
