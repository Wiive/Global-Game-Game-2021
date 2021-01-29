using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    public bool partOfMaze = false;
    public bool isWall = false;
    public Vector2Int exploredDirection;

    public bool isExplored = false;
    public MazeNode exploredFrom;

    public bool hasRelic = false;
    public bool isExit = false;

    public bool isEnemySpawner = false;
    public bool isPlayerSpawner = false;

    [SerializeField] Vector2Int gridPos = new Vector2Int();
    public Vector2Int GridPos { get { return gridPos; } set { gridPos = value; } }

    [SerializeField] Color32 blocked = new Color32();
    [SerializeField] Color32 nonBlocked = new Color32();
    [SerializeField] Color32 exit = new Color32();
    [SerializeField] Color32 relicPlaced = new Color32();


    [SerializeField] GameObject playerSpawn = null;
    [SerializeField] GameObject relicSpawn = null;
    [SerializeField] GameObject enemySpawn = null;


    private void FixedUpdate()
    {
        if(!isWall)
        {
            GetComponent<SpriteRenderer>().color = nonBlocked;
        }
        if (isWall)
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
    }

    public void SetNodeState()
    {
        if(hasRelic)
        {
            Instantiate(relicSpawn, transform.position, transform.rotation, transform);
        }
        if (isEnemySpawner)
        {
            Instantiate(enemySpawn, transform.position, transform.rotation, transform);
        }
        if (isPlayerSpawner)
        {
            Instantiate(playerSpawn, transform.position, transform.rotation, transform);
        }

    }
}
