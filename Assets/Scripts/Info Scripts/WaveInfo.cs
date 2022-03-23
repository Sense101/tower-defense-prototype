using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores a group of spawn groups to spawn in a full wave
/// </summary>
[System.Serializable]
public class WaveInfo
{
    // needs to store more info about the wave @TODO

    [Tooltip("the different groups of enemies to spawn in the wave")]
    [SerializeField] List<SpawnGroup> _spawnGroups = new List<SpawnGroup>();
    public List<SpawnGroup> SpawnGroups
    {
        get => _spawnGroups;
    }
}
