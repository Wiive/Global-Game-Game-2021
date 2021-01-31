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
}
