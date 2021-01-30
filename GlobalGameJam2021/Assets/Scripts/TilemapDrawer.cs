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
    [SerializeField] private Tilemap floorLayer;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase floorTile;

    private void Awake()
    {
        GetAllComponents();
    }

    private void GetAllComponents()
    {
        MazeCreator maze = FindObjectOfType<MazeCreator>();
        map = maze.Grid;
        mapSize = maze.GridSize;
    }

    public void PaintMap()
    {
        wallLayer.ClearAllTiles();
        floorLayer.ClearAllTiles();

        foreach (var node in map)
        {
            floorLayer.SetTile(node.Value.GridPos3, floorTile);
            
            if (node.Value.isWall)
                wallLayer.SetTile(node.Value.GridPos3, wallTile);
        }
        
        wallLayer.RefreshAllTiles();
    }
}
