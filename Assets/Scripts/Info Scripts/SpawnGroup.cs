using UnityEngine;

/// <summary>
/// stores a group of enemies to be spawned
/// </summary>
[System.Serializable]
public class SpawnGroup
{
    [Tooltip("the amount to spawn")]
    [SerializeField] int _spawnAmount = 10;
    public int SpawnAmount
    {
        get => _spawnAmount;
    }

    [Tooltip("info about the enemy to spawn")]
    [SerializeField] EnemyInfo _enemyInfo;
    public EnemyInfo EnemyInfo
    {
        get => _enemyInfo;
    }
}
