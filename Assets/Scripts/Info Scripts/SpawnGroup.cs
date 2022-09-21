using UnityEngine;

/// <summary>
/// stores a group of enemies to be spawned
/// </summary>
[System.Serializable]
public class SpawnGroup
{
    public float startingDelay = 0;

    [Tooltip("the amount to spawn")]
    public int spawnAmount = 0;

    [Tooltip("the enemy to spawn")]
    public Enemy enemyPrefab;

    //@TODO which spawner as well

    public float delayBetweenSpawns = 1;
}
