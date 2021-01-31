using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitDust : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;


    public void EmitDustCLoud()
    {
        _particleSystem.Emit(9);
    }

    private void Update()
    {
        if (GameStateManager.instance.CurrentGameState == GameStateManager.GameState.GameOver)
        {
            _particleSystem.GetComponent<ParticleSystemRenderer>().enabled = false;
        }
    }


}
