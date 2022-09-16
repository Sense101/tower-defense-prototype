using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// For the wave container prefab - contains all info regarding one wave container
/// </summary>
public class WaveContainer : MonoBehaviour
{
    // set in inspector
    public TextMeshProUGUI waveNumberText;
    public Transform rightContainer; // where enemy containers are spawned

    // prefab references
    public EnemyContainer enemyContainerPrefab;

    // variables
    public EnemyContainer[] enemyContainers;
}