using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public RuntimeAnimatorController runtimeAnimatorController;
}
