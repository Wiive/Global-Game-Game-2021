using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemy = null;

    MazeCreator mazeCreator;

    

    private void Awake()
    {
        mazeCreator = GetComponent<MazeCreator>();
    }

    IEnumerator Start()
    {
        yield return mazeCreator.GenerateMaze();
        Debug.Log("Job done");

        Instantiate(enemy, mazeCreator.startingPos(), transform.rotation);
    }
}
