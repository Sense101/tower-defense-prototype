using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class TurretInfo : ScriptableObject
{
    // the turret type, controls what it does to the turret stats
    public enum Type { multiplier, basic }

    public Type type = Type.multiplier;

    // Damage
    [Tooltip("the damage the turret does")]
    [SerializeField] float _damageModifier = 1;
    public float DamageModifier
    {
        get => _damageModifier;
    }


    // Range
    [Tooltip("the range of the turret in tiles")]
    [SerializeField] float _rangeModifier = 1;
    public float RangeModifier
    {
        get => _rangeModifier;
    }

    // Reload Time
    [Tooltip("the time it takes for a gun to reload in seconds")]
    [SerializeField] float _reloadTimeModifier = 1;
    public float ReloadTimeModifier
    {
        get => _reloadTimeModifier;
    }

    // Spin Speed
    [Tooltip("spin speed in degrees per second")]
    [SerializeField] float _spinSpeedModifier = 1;
    public float SpinSpeedModifier
    {
        get => _spinSpeedModifier;
    }

    [Space(5)]
    [Header("Sprites")]
    public Sprite fullSprite = default; // the full sprite, including the base
    public Sprite previewSprite = default; // without the base
    public Sprite turretBaseSprite = default; // just the base
    public Sprite bodySprite = default; // the body
    public Sprite gunMountSprite = default; // just the gun mount

    [Space(5)]
    public GunInfo[] guns;

    [Space(5)]
    public BulletInfo bulletInfo;

    [Space(5)]
    public TurretInfo[] mutationPaths;
}
