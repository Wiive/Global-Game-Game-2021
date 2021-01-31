using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] List<RelicPointerController> relicPointers = new List<RelicPointerController>();
    private List<MazeNode> huntSpawners = new List<MazeNode>();
    private List<Transform> playerSpawners = new List<Transform>();
    private List<Transform> relicSpawners = new List<Transform>();

    [SerializeField] int hunterInitialSpawn = 4;
    [SerializeField] int maxHunters = 10;
    [SerializeField] float timeInbetweenSpawns = 2;

    int currentlySpawned = 0;
    float currentTime = 0;

    List<Enemy> spawnedHunters = new List<Enemy>();

    public List<Enemy> SpawnedHunters => spawnedHunters;

    public Enemy enemyPrefab;
    public EnemyData[] enemiesData;
    public Player playerPrefab;
    private GameObject playerObject;

    public Relic[] relicPrefab;
    public RelicData[] relicsData;

    private void OnEnable()
    {
        GameStateManager.instance.onChangeGameState += OnGameStateChange;
    }
    private void OnDisable()
    {
        GameStateManager.instance.onChangeGameState -= OnGameStateChange;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (currentlySpawned >= maxHunters) return;
        currentTime += Time.deltaTime;
        if (currentTime < timeInbetweenSpawns) return;
        currentTime = 0;
        SpawnNewHunter();
    }

    public void SpawnEntities()
    {
        FindSpawners();
        SpawnHunter(hunterInitialSpawn);
        SpawnPlayer();
        SpawnRelic(relicSpawners.Count);

    }
    private void FindSpawners()
    {
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("HunterSpawner"))
        {
            huntSpawners.Add(spawnPoint.GetComponentInParent<MazeNode>());
        }
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("PlayerSpawner"))
        {
            playerSpawners.Add(spawnPoint.transform);
        }
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("RelicSpawner"))
        {
            relicSpawners.Add(spawnPoint.transform);
        }
    }
    private void SpawnHunter(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            currentlySpawned++;
            int spawnIndex = Random.Range(0, huntSpawners.Count);
            Enemy enemy = Instantiate(enemyPrefab, huntSpawners[spawnIndex].transform.position, transform.rotation,transform.parent);
            enemy.SetData(enemiesData[amount == enemiesData.Length ? i : Random.Range(0, enemiesData.Length-1)]);
            enemy.TileSize = huntSpawners[spawnIndex].TileSize;
            enemy.CurrentPos = huntSpawners[spawnIndex].GridPos;
            enemy.SpawnPoint = huntSpawners[spawnIndex];
            enemy.name = $"Enemy{spawnedHunters.Count}";
            spawnedHunters.Add(enemy);
        }
    }

    private void SpawnNewHunter()
    {
        currentlySpawned++;
        MazeNode hunterSpawnPoint = GetFurthestSpawnPoint();
        Enemy enemy = Instantiate(enemyPrefab, hunterSpawnPoint.transform.position, transform.rotation, transform.parent);
        enemy.SetData(enemiesData[Random.Range(0, enemiesData.Length - 1)]);
        enemy.TileSize = hunterSpawnPoint.TileSize;
        enemy.CurrentPos = hunterSpawnPoint.GridPos;
        enemy.SpawnPoint = hunterSpawnPoint;
        enemy.name = $"Enemy{spawnedHunters.Count}";
        spawnedHunters.Add(enemy);
    }

    public MazeNode GetSpawnPos()
    {
        return GetFurthestSpawnPoint();
    }

    private MazeNode GetFurthestSpawnPoint()
    {
        Vector2 playerPosition = playerObject.transform.position;
        
        float furthestDistance = 0;
        MazeNode returnValue = null;
        
        foreach (MazeNode spawnPoint in huntSpawners)
        {
            float distance = Vector2.Distance(playerPosition, spawnPoint.transform.position);

            if (distance > furthestDistance)
            {
                returnValue = spawnPoint;
                furthestDistance = distance;
            }
        }

        if(returnValue == null)
        {
            int index = Random.Range(0, huntSpawners.Count);
            returnValue = huntSpawners[index];
        }
        
        return returnValue;
    }
    
    public void SpawnPlayer()
    {
        //int prefabIndex = Random.Range(0, playerPrefab.Length);
        Player player = Instantiate(playerPrefab, playerSpawners[0].parent.transform.position, transform.rotation, transform.parent);
        player.TileSize = playerSpawners[0].GetComponentInParent<MazeNode>().TileSize;
        player.CurrentPos = playerSpawners[0].GetComponentInParent<MazeNode>().GridPos;
        playerObject = player.gameObject;
    }
    public void SpawnRelic(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 relicPos = relicSpawners[i].parent.transform.position;
            Relic relic = Instantiate(relicPrefab[0], relicPos, transform.rotation, transform.parent);

            relic.SpawnPoint = relicSpawners[i].parent.GetComponent<MazeNode>();
            relic.SetData(relicsData[i]);
            relicPointers[i].SetRelic(relic);

        }
    }

    void OnGameStateChange(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.GameLoop)
        {
            if (GameStateManager.instance.PreviousGameState == GameStateManager.GameState.MainMenu)
            {
   
            }
        }
    }
}
