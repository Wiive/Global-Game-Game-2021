using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreator : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new Vector2Int(0, 0);
    [SerializeField] bool generateGrid = false;
    [SerializeField] GameObject wayPoint = null;
    [SerializeField] int TileSize = 10;

    // Update is called once per frame
    void Update()
    {
        if(generateGrid)
        {
            GenerateGrid();
        }
    }
    
    private void GenerateGrid()
    {
        WayPoint[] children = GetComponentsInChildren<WayPoint>();
        foreach (var child in children)
        {
            DestroyImmediate(child.transform.gameObject);
        }

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xPos = transform.position.x + (x * TileSize);
                float yPos = transform.position.y + (y * TileSize);

                GameObject newWaypoint = Instantiate(wayPoint, new Vector3(xPos, yPos, 0),transform.rotation,transform);
                newWaypoint.GetComponent<WayPoint>().GridPos = new Vector2Int(x, y);
                newWaypoint.name = x + " , " + y;
            }
        }
        generateGrid = false;
    }
}
