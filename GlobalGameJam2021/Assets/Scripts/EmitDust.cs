using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitDust : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    public void EmitDustCLoud()
    {
        _particleSystem.Emit(9);
    }
}
