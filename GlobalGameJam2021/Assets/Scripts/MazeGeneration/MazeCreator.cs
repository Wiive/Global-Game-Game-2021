using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new Vector2Int(0, 0);
    [SerializeField] MazeNode node = null;
    [SerializeField] int TileSize = 16;

    [SerializeField] int roomSize = 1;
    [SerializeField] int relicsToPlace = 4;
    [SerializeField] int safeZone = 10;
    [SerializeField] int borderZone = 4;


    [SerializeField] private SpriteRenderer gridRenderer;


    MazeNode[,] mazeModell;

    [SerializeField] List<MazeNode> frontier = new List<MazeNode>();
    [SerializeField] List<MazeNode> neighbours = new List<MazeNode>();
    [SerializeField] List<MazeNode> spawnedRelics = new List<MazeNode>();

    List<MazeNode>[] squareNodes = new List<MazeNode>[]
    {
        new List<MazeNode>(),
        new List<MazeNode>(),
        new List<MazeNode>(),
        new List<MazeNode>()
    };

    Vector2Int[] directions =
    {
        new Vector2Int(0, -2),
        new Vector2Int(0, 2),
        new Vector2Int(2, 0),
        new Vector2Int(-2, 0)
    };

    Vector2Int[] roomCreation =
    {
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1)
    };

    private BoxCollider2D mapBorder;


    private void Awake()
    {
        mapBorder = GetComponent<BoxCollider2D>();
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        mapBorder.size = new Vector2(gridSize.x * TileSize, gridSize.y * TileSize);
        mapBorder.offset = new Vector2(mapBorder.size.x / 2, mapBorder.size.y / 2);

        gridRenderer.size = mapBorder.size;

        mazeModell = new MazeNode[gridSize.x, gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xPos = transform.position.x + (x * TileSize);
                float yPos = transform.position.y + (y * TileSize);

                MazeNode mazeNode = Instantiate(node, new Vector3(xPos, yPos, 0), transform.rotation, transform);

                // check if we are on the left side of the maze
                if(x < gridSize.x/2 )
                {
                    // check if we are below the halfwaypoint on Y
                    if(y < gridSize.y/2)
                    {
                        // add to squareNodes 0 (left down corner)
                        squareNodes[0].Add(mazeNode);
                    }
                    // else we are above the halfwaypoint on Y
                    else
                    {
                        // add to squareNodes 1 (left upper corner)
                        squareNodes[1].Add(mazeNode);
                    }
                }
                // else we are on the right side
                else
                {
                    // check if we are below the halfwaypoint on Y
                    if (y < gridSize.y / 2)
                    {
                        // add to squareNodes 2 (right down corner)
                        squareNodes[2].Add(mazeNode);
                    }
                    // else we are above the halfwaypoint on Y
                    else
                    {
                        // add to squareNodes 3 (right upper corner)
                        squareNodes[3].Add(mazeNode);
                    }
                }


                mazeNode.isWall = false;
                if (y == gridSize.y - 1 || y == 0)
                    mazeNode.isWall = true;
                if(x == gridSize.x -1 || x == 0)
                    mazeNode.isWall = true;

                mazeNode.GridPos = new Vector2Int(x, y);
                mazeNode.name = x + " , " + y;
                mazeModell[x, y] = mazeNode;
            }
        }
    }

    public IEnumerator GenerateMaze()
    {
        int x = Random.Range(1, mazeModell.GetLength(0) - 2);
        int y = Random.Range(1, mazeModell.GetLength(1) - 2);
        mazeModell[2, 2].isWall = true;
        AddFrontierCells(mazeModell[2, 2]);

        while (frontier.Count > 0)
        {
            int random = Random.Range(0, frontier.Count);

            MazeNode node = frontier[random];

            //yield return new WaitForSeconds(0);

            node.isWall = true;
            node.partOfMaze = true;

            MazeNode nodeNeighbour = GetRandomNeighbour(node);
            if (nodeNeighbour != null)
                ConnectNodes(node, nodeNeighbour);

            //yield return new WaitForSeconds(0);

            AddFrontierCells(node);

            frontier.Remove(node);
            frontier.RemoveAll(node => node == null);
            if (frontier.Count <= 0)
                break;
        }

        yield return new WaitForSeconds(0);
        SetRelicPosition();
        SetPlayerSpawn();
        SetEnemySpawn();
        
    }
    private MazeNode GetRandomNeighbour(MazeNode frontier)
    {
        neighbours = new List<MazeNode>();
        foreach (var direction in directions)
        {
            Vector2Int Cords = frontier.GridPos + direction;

            if (IsValidFrontier(Cords.x, Cords.y))
            {
                neighbours.Add(mazeModell[Cords.x, Cords.y]);
                mazeModell[Cords.x, Cords.y].exploredDirection = direction;
            }
        }

        if (neighbours.Count < 1)
        {
            Debug.Log("To few neighbours");
            return null;
        }

        int random = Random.Range(0, neighbours.Count);

        return neighbours[random];
    }
    private void ConnectNodes (MazeNode frontier, MazeNode neighbour)
    {
            Vector2Int inbetweenPos = frontier.GridPos + (neighbour.exploredDirection / 2);
            mazeModell[inbetweenPos.x, inbetweenPos.y].isWall = true;
    }
    private void AddFrontierCells(MazeNode node)
    {
        foreach (var direction in directions)
        {
            Vector2Int Cords = node.GridPos + direction;         

            if (IsValidFrontier(Cords.x ,Cords.y))
            {
                if (!mazeModell[Cords.x, Cords.y].partOfMaze && !frontier.Contains(mazeModell[Cords.x,Cords.y]))
                {
                    frontier.Add(mazeModell[Cords.x, Cords.y]);
                }
            }
        }
    }
    private bool IsValidFrontier(int x, int y)
    {

        if (x > mazeModell.GetLength(0) - 3) return false;
        if (x < 2) return false;
        if (y > mazeModell.GetLength(1) - 3) return false;
        if (y < 2) return false;
        return true;
    }


    private void SetRelicPosition()
    {
        // make this spawn a relic in each corner 
        //(split the map in four and then random in that section)
        for (int i = 0; i < relicsToPlace; i++)
        {
            MazeNode relicSpot = GetRelicSpot(i);
            CreateRoom(relicSpot);
        }
    }

    private MazeNode GetRelicSpot(int index)
    {
        int bottomX;
        int topX;
        int bottomY;
        int topY;
        switch (index)
        {
            case 0:
                bottomX = borderZone;
                topX = gridSize.x / 2 - borderZone;
                bottomY = borderZone;
                topY = gridSize.y / 2 - borderZone;
                break;
            case 1:
                bottomX = borderZone;
                topX = gridSize.x / 2 - borderZone;
                bottomY = gridSize.y / 2 + borderZone;
                topY = gridSize.y - borderZone;
                break;
            case 2:
                bottomX = gridSize.x / 2 + borderZone;
                topX = gridSize.x - borderZone;
                bottomY = borderZone;
                topY = gridSize.y / 2 - borderZone;
                break;
            default:
                bottomX = gridSize.x / 2 + borderZone;
                topX = gridSize.x - borderZone;
                bottomY = gridSize.y / 2 + borderZone;
                topY = gridSize.y - borderZone;
                break;
        }



        // divide the maze into 4 sections
        MazeNode relicSpot;
        int x;
        int y;
        do
        {
            int random = Random.Range(0,squareNodes[index].Count);
            Debug.Log(random);

            relicSpot = squareNodes[index][random];

            Debug.Log(relicSpot.GridPos);

            x = relicSpot.GridPos.x;
            Debug.Log(x);
            y = relicSpot.GridPos.y;
            Debug.Log(y);

        }
        while (x < bottomX || x > topX || y < bottomY || y > topY);

        relicSpot.hasRelic = true;
        relicSpot.isWall = false;
        relicSpot.SetNodeState();
        spawnedRelics.Add(relicSpot);
        return relicSpot;
    }

    private void CreateRoom(MazeNode relicSpot)
    {
        foreach (var direction in roomCreation)
        {
            Vector2Int nodePos = new Vector2Int(relicSpot.GridPos.x + direction.x, relicSpot.GridPos.y + direction.y);
            mazeModell[nodePos.x, nodePos.y].isWall = false;
        }
    }


    private void SetPlayerSpawn()
    {
        int nodeIndex = Random.Range(0, spawnedRelics.Count);
        int roomIndex = Random.Range(0, roomCreation.Length);
        Vector2Int pos = spawnedRelics[nodeIndex].GridPos + roomCreation[roomIndex];
        mazeModell[pos.x, pos.y].isPlayerSpawner = true;
        mazeModell[pos.x, pos.y].SetNodeState();
    }
    

    private void SetExits()
    {
        // set a exit point on each side, depending on where the relics are.
    }

    private void SetEnemySpawn()
    {
        // make enemies spawn along the borders close to exit?

        for (int i = 0; i < 5; i++)
        {
            int x;
            int y;

            do
            {
                x = Random.Range(borderZone, mazeModell.GetLength(0) - borderZone);
                y = Random.Range(borderZone, mazeModell.GetLength(1) - borderZone);
            }
            while (mazeModell[x, y].hasRelic || mazeModell[x, y].isPlayerSpawner || mazeModell[x, y].isWall || mazeModell[x, y].isEnemySpawner);

            mazeModell[x, y].isEnemySpawner = true;
            mazeModell[x, y].SetNodeState();
        }
    }

    public Vector3 startingPos()
    {
        // make this more fancy so that it wont spawn hunters on relics or to close.
        MazeNode node;
        do
            node = mazeModell[Random.Range(1, mazeModell.GetLength(0) - 1), Random.Range(1, mazeModell.GetLength(1) - 1)];
        while (node.isWall);

        Vector3 position = new Vector3(node.transform.position.x , node.transform.position.y, 0);

        return position;
    }
}

