using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public RuntimeAnimatorController runtimeAnimatorController;
    public enum SpriteSheets
    {
        A,
        B,
        C,
        D
    }
    public SpriteSheets currentSpriteSheet;

}
