using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class TurretInfo : ScriptableObject
{
    public enum FiringState { none, aiming, firing }

    // WIP @TODO
    public int damage = 0;


    [Tooltip("the range of the turret in tiles")]
    public float range = 0;

    [Tooltip("the time it takes for a gun to reload in seconds")]
    [Range(0.1f, 10)] public float reloadTime = 1;

    [Tooltip("the time it takes to turn a full 360 degrees in seconds")]
    [Range(1, 360)] public int spinSpeed = 90;
}
