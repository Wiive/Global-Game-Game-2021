using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreator : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new Vector2Int(0, 0);
    [SerializeField] bool generateGrid = false;
    [SerializeField] GameObject node = null;
    [SerializeField] int TileSize = 16;
    [SerializeField] private Transform nodeParent;
    [SerializeField] private SpriteRenderer gridRenderer;
    private BoxCollider2D mapBorder;

    private void Awake()
    {
        GetAllComponents();
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (generateGrid)
        {
            GenerateGrid();
        }
    }

    private void GetAllComponents()
    {
        mapBorder = GetComponent<BoxCollider2D>();
    }
    
    private void GenerateGrid()
    {
        MazeNode[] children = GetComponentsInChildren<MazeNode>();
        foreach (var child in children)
        {
            DestroyImmediate(child.transform.gameObject);
        }

        mapBorder.size = new Vector2(gridSize.x * TileSize, gridSize.y * TileSize);
        mapBorder.offset = new Vector2(mapBorder.size.x / 2, mapBorder.size.y / 2);
        
        gridRenderer.size = mapBorder.size;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xPos = transform.position.x + (x * TileSize);
                float yPos = transform.position.y + (y * TileSize);

                GameObject newWaypoint = Instantiate(node, new Vector3(xPos, yPos, 0),Quaternion.identity , nodeParent);
                newWaypoint.GetComponent<MazeNode>().GridPos = new Vector2Int(x, y);
                newWaypoint.name = x + " , " + y;
            }
        }
        generateGrid = false;
    }
}
