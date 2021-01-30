using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
	Dictionary<Vector2Int, MazeNode> grid = new Dictionary<Vector2Int, MazeNode>();
	public Dictionary<Vector2Int, MazeNode> Grid { get { return grid; } }

	[SerializeField] Vector2Int gridSize = new Vector2Int(0, 0);
	public Vector2Int GridSize { get { return gridSize; } }
	[SerializeField] MazeNode node = null;
	[SerializeField] int tileSize = 16;

	[SerializeField] int relicsToPlace = 4;
	[SerializeField] int borderZone = 4;

	[Tooltip("This number determines how many enemie spawnPoints there will be on each side")]
	[SerializeField] int spawnOnEachSide = 2;


	[SerializeField] private SpriteRenderer gridRenderer;


	MazeNode[,] mazeModell;

	[SerializeField] Queue<MazeNode> frontier = new Queue<MazeNode>();
	[SerializeField] List<MazeNode> neighbours = new List<MazeNode>();
	[SerializeField] List<MazeNode> spawnedRelics = new List<MazeNode>();

	List<MazeNode>[] squareNodes = new List<MazeNode>[]
	{
		new List<MazeNode>(),
		new List<MazeNode>(),
		new List<MazeNode>(),
		new List<MazeNode>()
	};

	List<MazeNode>[] borders =
	{
		new List<MazeNode>(), // north
		new List<MazeNode>(), // east
		new List<MazeNode>(), // south
		new List<MazeNode>()  // west
	};

	[SerializeField] List<MazeNode> northBorder = new List<MazeNode>();
	[SerializeField] List<MazeNode> eastBorder = new List<MazeNode>();
	[SerializeField] List<MazeNode> southBorder = new List<MazeNode>();
	[SerializeField] List<MazeNode> westBorder = new List<MazeNode>();

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
		mapBorder.size = new Vector2(gridSize.x * tileSize, gridSize.y * tileSize);
		mapBorder.offset = new Vector2(mapBorder.size.x / 2, mapBorder.size.y / 2);

		gridRenderer.size = mapBorder.size;

		mazeModell = new MazeNode[gridSize.x, gridSize.y];
		for (int y = 0; y < gridSize.y; y++)
		{
			for (int x = 0; x < gridSize.x; x++)
			{
				float xPos = transform.position.x + (x * tileSize);
				float yPos = transform.position.y + (y * tileSize);

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
				mazeNode.TileSize = tileSize;
				mazeNode.SetShadowCaster();
				mazeModell[x, y] = mazeNode;

				grid.Add(new Vector2Int(x, y), mazeNode);
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

			//MazeNode node = frontier[random];
			MazeNode node = frontier.Dequeue();

			//yield return new WaitForSeconds(0);

			node.isWall = true;
			node.SetShadowCaster();
			node.partOfMaze = true;

			MazeNode nodeNeighbour = GetRandomNeighbour(node);
			if (nodeNeighbour != null)
				ConnectNodes(node, nodeNeighbour);

			//yield return new WaitForSeconds(0);

			AddFrontierCells(node);

  /*          frontier.Remove(node);
			frontier.RemoveAll(node => node == null);*/
			if (frontier.Count <= 0)
				break;
		}

		yield return new WaitForSeconds(0);
		SetRelicPosition();
		yield return new WaitForSeconds(0);
		SetPlayerSpawn();
		yield return new WaitForSeconds(0);
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
	private void ConnectNodes(MazeNode frontier, MazeNode neighbour)
	{
		Vector2Int inbetweenPos = frontier.GridPos + (neighbour.exploredDirection / 2);
		mazeModell[inbetweenPos.x, inbetweenPos.y].isWall = true;
		mazeModell[inbetweenPos.x, inbetweenPos.y].SetShadowCaster();
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
					//frontier.Add(mazeModell[Cords.x, Cords.y]);'
					frontier.Enqueue(mazeModell[Cords.x, Cords.y]);
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

		Vector2Int bottomBounds;
		Vector2Int topBounds;
		switch (index)
		{
			case 0:
				bottomBounds = new Vector2Int(borderZone,borderZone);
				topBounds = new Vector2Int(gridSize.x / 2 - borderZone, gridSize.y / 2 - borderZone);
				break;
			case 1:
				bottomBounds = new Vector2Int(borderZone, gridSize.y / 2 + borderZone);
				topBounds = new Vector2Int(gridSize.x / 2 - borderZone, gridSize.y - borderZone);
				break;
			case 2:
				bottomBounds = new Vector2Int(gridSize.x / 2 + borderZone, borderZone);
				topBounds = new Vector2Int(gridSize.x - borderZone, gridSize.y / 2 - borderZone);
				break;
			default:
				bottomBounds = new Vector2Int(gridSize.x / 2 + borderZone, gridSize.y / 2 + borderZone);
				topBounds = new Vector2Int(gridSize.x - borderZone, gridSize.y - borderZone);
				break;
		}

		MazeNode relicSpot;

		int x;
		int y;
		int indexTry = 0;
		do
		{
			int random = Random.Range(0,squareNodes[index].Count);

			relicSpot = squareNodes[index][random];

			x = relicSpot.GridPos.x;
			y = relicSpot.GridPos.y;
			indexTry++;
			if(indexTry > 5)
            {
				relicSpot = FindFirstValidSpawnPos(squareNodes[index],bottomBounds,topBounds);
				break;
            }
		}
		while ((x < bottomBounds.x || x > topBounds.x || y < bottomBounds.y || y > topBounds.y));

		relicSpot.hasRelic = true;
		relicSpot.isWall = false;
		relicSpot.SetShadowCaster();
		relicSpot.SetNodeState();
		spawnedRelics.Add(relicSpot);
		return relicSpot;
	}

	private MazeNode FindFirstValidSpawnPos(List<MazeNode> nodes, Vector2Int bottomBounds, Vector2Int topBounds)
    {
        foreach (var node in nodes)
        {
			Vector2Int pos = node.GridPos;
			if (pos.x > bottomBounds.x && pos.x < topBounds.x && pos.y > bottomBounds.y && pos.y < topBounds.y)
				return node;
        }
		Debug.LogError("No match found");
		return null;
    }

	private void CreateRoom(MazeNode roomCenter)
	{
		foreach (var direction in roomCreation)
		{
			// add a check to se if the node next to it is valid
			Vector2Int nodePos = new Vector2Int(roomCenter.GridPos.x + direction.x, roomCenter.GridPos.y + direction.y);
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
	
	// this might not be needed.
	private void SetExits()
	{
		// set a exit point on each side, depending on where the relics are.
	}

	private void SetEnemySpawn()
	{
		foreach (var item in grid)
		{
			Vector2Int pos = item.Value.GridPos;
			if (pos.y == gridSize.y - 2 && pos.x != gridSize.x - 1 && pos.x != 0)
				borders[0].Add(item.Value);
			else if (pos.x == gridSize.x - 2 && pos.y != gridSize.y - 1 && pos.y != 0)
				borders[1].Add(item.Value);
			else if (pos.y == 1 && pos.x != gridSize.x - 1 && pos.x != 0)
				borders[2].Add(item.Value);
			else if (pos.x == 1 && pos.y != gridSize.y - 1 && pos.y != 0)
				borders[3].Add(item.Value);
		}

		int random = 0;
        for (int i = 0; i < borders.Length; i++)
        {
            for (int j = 0; j < spawnOnEachSide; j++)
            {
                random = Random.Range(0, borders[i].Count);
                borders[i][random].isEnemySpawner = true;
                borders[i][random].SetNodeState();
            }
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

