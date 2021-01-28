using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public bool isExplored = false;
    public bool addedToGrid = false;
    public WayPoint exploredFrom;
    public bool isBlocked = false;
    int size = 10;

    [SerializeField] Vector2Int gridPos = new Vector2Int();
    public Vector2Int GridPos { get { return gridPos; } set { gridPos = value; } }

    [SerializeField] Color32 blocked = new Color32();
    [SerializeField] Color32 nonBlocked = new Color32();

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


    // remove color setting its just for debugging.
    private void Start()
    {
        if (isBlocked)
        {
            GetComponent<SpriteRenderer>().color = blocked;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = nonBlocked;
        }
    }
}
