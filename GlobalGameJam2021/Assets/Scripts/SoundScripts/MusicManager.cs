using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource audioSource;
    public AudioClip menuMusic;
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
        GameStateManager.instance.onChangeGameState += ChangeMusicBasedOnState;
    }

    private void OnDisable()
    {
        GameStateManager.instance.onChangeGameState -= ChangeMusicBasedOnState;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ChangeMusicBasedOnState(GameStateManager.instance.CurrentGameState);

    }

    void ChangeMusicBasedOnState(GameStateManager.GameState newState)
    {
        switch (newState)
        {
            case GameStateManager.GameState.MainMenu:
                audioSource.Stop();
                PlayMenuMusic();
                break;
            case GameStateManager.GameState.IngameMenu:
                break;
            case GameStateManager.GameState.GameLoop:
                if (GameStateManager.instance.PreviousGameState == GameStateManager.GameState.MainMenu)
                {
                    audioSource.Stop();
                    PlayLevelMusic();
                }
                
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
    public void PlayMenuMusic()
    {

        audioSource.clip = menuMusic;
        audioSource.Play();
    }

}
