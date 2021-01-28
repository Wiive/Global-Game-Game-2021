using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public bool isExplored = false;
    public WayPoint exploredFrom;
    public bool isBlocked = false;
    public bool hasRelic = false;
    public bool isExit = false;
    int size = 10;

    [SerializeField] Vector2Int gridPos = new Vector2Int();
    public Vector2Int GridPos { get { return gridPos; } set { gridPos = value; } }

    [SerializeField] Color32 blocked = new Color32();
    [SerializeField] Color32 nonBlocked = new Color32();
    [SerializeField] Color32 exit = new Color32();
    [SerializeField] Color32 relicPlaced = new Color32();

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
        else if (isExit)
        {
            GetComponent<SpriteRenderer>().color = exit;
        }
        else if (hasRelic)
        {
            GetComponent<SpriteRenderer>().color = relicPlaced;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = nonBlocked;
        }
    }
}
