using UnityEngine;

/// <summary>
/// all the info about a given enemy type
/// </summary>

[System.Serializable]
[CreateAssetMenu(menuName = "Infos/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [Tooltip("move speed in tiles/sec")]
    public float moveSpeed = 1.5f;

    [Tooltip("health in ???")]
    public int health = 30;

    [Tooltip("@todo set up armor system")]
    public int armor = 0;

    public int killReward = 0;
}
