using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource audioSource;
    public AudioClip levelMusic;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }
    private void OnEnable()
    {
        GameStateManager.instance.onChangeGameState += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.instance.onChangeGameState -= OnGameStateChange;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        switch (GameStateManager.instance.CurrentGameState)
        {
            case GameStateManager.GameState.MainMenu:
                break;
            case GameStateManager.GameState.IngameMenu:
                break;
            case GameStateManager.GameState.GameLoop:
                PlayLevelMusic();
                break;
            case GameStateManager.GameState.GameOver:
                break;
            case GameStateManager.GameState.Victory:
                break;
            default:
                break;
        }

    }

    void OnGameStateChange(GameStateManager.GameState newState)
    {
        switch (newState)
        {
            case GameStateManager.GameState.MainMenu:
                audioSource.Stop();
                break;
            case GameStateManager.GameState.IngameMenu:
                break;
            case GameStateManager.GameState.GameLoop:
                audioSource.Stop();
                PlayLevelMusic();
                break;
            case GameStateManager.GameState.GameOver:
                audioSource.Stop();
                break;
            case GameStateManager.GameState.Victory:
                break;
            default:
                break;
        }
    }

    public void PlayLevelMusic()
    {
        audioSource.clip = levelMusic;
        audioSource.Play();
    }

}
