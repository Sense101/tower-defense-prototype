using UnityEngine;

/// <summary>
/// all the info about a given enemy type
/// </summary>

[System.Serializable]
[CreateAssetMenu]
public class EnemyInfo : ScriptableObject
{
    [Tooltip("move speed in tiles/sec")]
    [SerializeField] float _moveSpeed = 1.5f;
    public float MoveSpeed
    {
        get => _moveSpeed;
    }

    [Tooltip("health in ???")]
    [SerializeField] int _health = 30;
    public int Health
    {
        get => _health;
    }

    [Tooltip("damage reduction per hit")]
    [SerializeField] int _armor = 0;
    public int Armor
    {
        get => _armor;
    }

    [Header("Rendering")]
    public Sprite sprite;
}
