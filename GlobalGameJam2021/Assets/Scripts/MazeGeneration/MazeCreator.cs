using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new Vector2Int(0, 0);
    [SerializeField] MazeNode node = null;
    [SerializeField] int TileSize = 16;

    MazeNode[,] mazeModell;

    [SerializeField] List<MazeNode> frontier = new List<MazeNode>();
    [SerializeField] List<MazeNode> neighbours = new List<MazeNode>();

    Vector2Int[] directions =
    {
        new Vector2Int(0, -2),
        new Vector2Int(0, 2),
        new Vector2Int(2, 0),
        new Vector2Int(-2, 0)
    };


    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        DestroyPrevious();

        mazeModell = new MazeNode[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xPos = transform.position.x + (x * TileSize -8);
                float yPos = transform.position.y + (y * TileSize);

                MazeNode mazeNode = Instantiate(node, new Vector3(xPos, yPos, 0), transform.rotation, transform);

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

    private void DestroyPrevious()
    {
        MazeNode[] children = GetComponentsInChildren<MazeNode>();
        foreach (var child in children)
        {
            DestroyImmediate(child.transform.gameObject);
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

            yield return new WaitForSeconds(0);
            int random = Random.Range(0, frontier.Count);

            MazeNode node = frontier[random];
            //MazeNode node = frontier.Dequeue();
            node.isWall = true;
            node.partOfMaze = true;

            yield return new WaitForSeconds(0);
            MazeNode nodeNeighbour = GetRandomNeighbour(node);
            if (nodeNeighbour != null)
                ConnectNodes(node, nodeNeighbour);

            AddFrontierCells(node);

            frontier.Remove(node);
            frontier.RemoveAll(node => node == null);
        }
        Debug.Log("Maze Done");
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
                //mazeModell[Cords.x, Cords.y].hasRelic = true;
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
        //neighbour.isWall = false;
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
                    //frontier.Enqueue(mazeModell[Cords.x,Cords.y]);
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


    public Vector3 startingPos()
    {
        MazeNode node;
        do
            node = mazeModell[Random.Range(1, mazeModell.GetLength(0) - 1), Random.Range(1, mazeModell.GetLength(1) - 1)];
        while (node.isWall);

        Vector3 position = new Vector3(node.transform.position.x - TileSize / 2, node.transform.position.y, 0);

        return position;
    }
}

