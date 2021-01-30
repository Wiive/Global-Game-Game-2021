using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] List<RelicPointerController> relicPointers = new List<RelicPointerController>();
    [SerializeField] private List<Transform> huntSpawners = new List<Transform>();
    [SerializeField] private List<Transform> playerSpawners = new List<Transform>();
    [SerializeField] private List<Transform> relicSpawners = new List<Transform>();

    [SerializeField] int hunterSpawnAmount = 4;

    public Enemy[] hunterPrefab;
    public Player playerPrefab;

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

    public void SpawnEntities()
    {
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("HunterSpawner"))
        {
            huntSpawners.Add(spawnPoint.transform);
        }

        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("PlayerSpawner"))
        {
            playerSpawners.Add(spawnPoint.transform);
        }
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("RelicSpawner"))
        {
            relicSpawners.Add(spawnPoint.transform);
        }

        SpawnHunter(hunterSpawnAmount);
        SpawnPlayer();
        SpawnRelic(relicSpawners.Count);

    }

    public void SpawnHunter(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int spawnIndex = Random.Range(0, huntSpawners.Count);
            //int prefabIndex = Random.Range(0, hunterPrefab.Length);
            Enemy enemy = Instantiate(hunterPrefab[0], huntSpawners[spawnIndex].parent.transform.position, transform.rotation,transform.parent);
            enemy.TileSize = huntSpawners[spawnIndex].GetComponentInParent<MazeNode>().TileSize;
            enemy.CurrentPos = huntSpawners[spawnIndex].GetComponentInParent<MazeNode>().GridPos;
        }
    }

    public void SpawnPlayer()
    {
        //int prefabIndex = Random.Range(0, playerPrefab.Length);
        Player player = Instantiate(playerPrefab, playerSpawners[0].parent.transform.position, transform.rotation, transform.parent);
        player.TileSize = playerSpawners[0].GetComponentInParent<MazeNode>().TileSize;
        player.CurrentPos = playerSpawners[0].GetComponentInParent<MazeNode>().GridPos;
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
