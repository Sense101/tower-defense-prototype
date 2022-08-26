﻿using UnityEngine;

/// <summary>
/// stores a group of enemies to be spawned
/// </summary>
[System.Serializable]
public class SpawnGroup
{
    [Tooltip("the amount to spawn")]
    public int spawnAmount = 10;

    [Tooltip("the enemy to spawn")]
    public GameObject enemyPrefab;

    [Header("Delays")]
    public float delayBeforeSpawn = 1;
    public float delayBetweenSpawns = 1;
}
