using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private List<Transform> huntSpawners = new List<Transform>();
    [SerializeField] private List<Transform> playerSpawners = new List<Transform>();
    [SerializeField] private List<Transform> relicSpawners = new List<Transform>();

    public GameObject[] hunterPrefab;
    public GameObject playerPrefab;
    public GameObject[] relicPrefab;

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

    private void Start()
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
    }

    public void SpawnHunter(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomSpawnIndex = Random.Range(0, huntSpawners.Count);
            int randomHunterIndex = Random.Range(0, hunterPrefab.Length);
            Instantiate(hunterPrefab[randomSpawnIndex], huntSpawners[randomSpawnIndex]);
        }
    }

    public void SpawnPlayer()
    {
        int randomSpawnIndex = Random.Range(0, huntSpawners.Count);
        int randomHunterIndex = Random.Range(0, hunterPrefab.Length);
        Instantiate(hunterPrefab[randomSpawnIndex], huntSpawners[randomSpawnIndex]);
    }

    public void SpawnRelic(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomSpawnIndex = Random.Range(0, relicSpawners.Count);
            int randomHunterIndex = Random.Range(0, relicPrefab.Length);
            Instantiate(hunterPrefab[randomSpawnIndex], relicSpawners[randomSpawnIndex]);
        }
    }

    void OnGameStateChange(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.GameLoop)
        {
            if (GameStateManager.instance.PreviousGameState == GameStateManager.GameState.MainMenu)
            {
                SpawnPlayer();
                SpawnHunter(5);
                SpawnRelic(4);
            }
        }
    }
}
