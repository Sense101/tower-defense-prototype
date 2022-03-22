using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores a group of enemies to be spawned
/// </summary>
[System.Serializable]
public class SpawnGroup
{
    public int spawnAmount;
    public GameObject enemyPrefab;
}
