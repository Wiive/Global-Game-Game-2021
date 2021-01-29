using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitDust : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void EmitDustCLoud()
    {
        particleSystem.Emit(9);
    }
}
