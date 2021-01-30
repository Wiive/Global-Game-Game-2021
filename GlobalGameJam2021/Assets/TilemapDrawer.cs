using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrawer : MonoBehaviour
{
    Dictionary<Vector2Int, MazeNode> map = new Dictionary<Vector2Int, MazeNode>();
    Vector2Int mapSize = new Vector2Int(0, 0);
    
    [SerializeField] private Tilemap wallLayer;
    [SerializeField] private TileBase wallTile;

    private void Awake()
    {
        GetAllComponents();
    }

    private void Start()
    {
        // ClearMap();
    }

    private void GetAllComponents()
    {
        MazeCreator maze = FindObjectOfType<MazeCreator>();
        map = maze.Grid;
        mapSize = maze.GridSize;
    }

    public void PaintMap()
    {
        // Debug.Log($"mapSize: {wallLayer.size}");

        // for (int y = 0; y < wallLayer.size.y; y++)
        // {
        //     for (int x = 0; x < wallLayer.size.x; x++)
        //     {
        //         Vector3Int tilePos = new Vector3Int(x, y, 0);
        //         TileBase tile = wallLayer.GetTile(tilePos);
        //         
        //         Debug.Log($"map[{y}][{x}]: {tile}");
        //     }
        // }
        
        wallLayer.ClearAllTiles();

        Vector3Int tilePos = new Vector3Int(0, 0, 0);
        // Tile tile = wallTile;
        // RuleTile rTile;
        // wallLayer.SetTile(tilePos, wallTile);
        // wallLayer.RefreshAllTiles();
        
        foreach (var node in map)
        {
            // if (node.Value.isWall) continue;
            // node.Value.
            if (node.Value.isWall)
            {
                // Debug.Log($"map[{node.Value.GridPos.y}][{node.Value.GridPos.x}]: isWall!");
                wallLayer.SetTile(node.Value.GridPos3, wallTile);
            }
        }
        
        wallLayer.RefreshAllTiles();
    }
}
