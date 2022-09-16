using UnityEngine;

/// <summary>
/// all the info about a given enemy type
/// </summary>

[System.Serializable]
[CreateAssetMenu(menuName = "Infos/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [Tooltip("move speed in tiles/sec")]
    [Min(0)] public float moveSpeed = 1.5f;

    [Tooltip("health in ???")]
    [Min(0)] public int health = 30;

    [Tooltip("How much armor the enemy has")]
    [Min(0)] public int armor = 0;

    [Tooltip("How much damage the armor reduces")]
    [Min(0)] public int armorStrength = 0;

    [Min(0)] public int killReward = 0;

    public Sprite previewSprite;
}
