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
        Debug.Log(playerSpawners.Count);
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("RelicSpawner"))
        {
            relicSpawners.Add(spawnPoint.transform);
        }

        SpawnHunter(huntSpawners.Count);
        SpawnPlayer();
        SpawnRelic(relicSpawners.Count);

    }

    public void SpawnHunter(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomSpawnIndex = Random.Range(0, huntSpawners.Count);
            //int randomHunterIndex = Random.Range(0, hunterPrefab.Length);
            Instantiate(hunterPrefab[0], huntSpawners[randomSpawnIndex].parent.transform.position, transform.rotation,transform.parent);
        }
    }

    public void SpawnPlayer()
    {
        int randomSpawnIndex = Random.Range(0, playerSpawners.Count);
        //int randomHunterIndex = Random.Range(0, hunterPrefab.Length);
        Instantiate(playerPrefab, playerSpawners[randomSpawnIndex].parent.transform.position, transform.rotation, transform.parent);
    }

    public void SpawnRelic(int amount)
    {
        for (int i = 0; i < amount; i++)
        {

            int randomSpawnIndex = Random.Range(0, relicSpawners.Count);
            //int randomHunterIndex = Random.Range(0, relicPrefab.Length);
            GameObject newRelic = Instantiate(relicPrefab[0], relicSpawners[randomSpawnIndex].parent.transform.position, transform.rotation, transform.parent);
            relicPointers[i].target = newRelic.GetComponent<Relic>();

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
