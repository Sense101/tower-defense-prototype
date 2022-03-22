using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// all the info about a given enemy type
/// </summary>

[System.Serializable]
[CreateAssetMenu]
public class EnemyInfo : ScriptableObject
{
    public enum PathSide { left, right }
    // move speed, health, armor, xp, extra stuff

    public enum TurnProgress { none, starting, turning, ending }

    [Tooltip("move speed in tiles/sec")]
    public float moveSpeed = 1;

    [Tooltip("health in ???")]
    public int health = 50;

    [Tooltip("armor in ???")]
    public int armor = 0;
}
