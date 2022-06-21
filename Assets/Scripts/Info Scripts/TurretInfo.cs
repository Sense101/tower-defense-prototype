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

    // Reload Speed
    [Tooltip("the time it takes for a gun to reload in seconds")]
    [SerializeField] float _reloadSpeedModifier = 1;
    public float ReloadSpeedModifier
    {
        get => _reloadSpeedModifier;
    }

    // Spin Speed
    [Tooltip("spin speed in degrees per second")]
    [SerializeField] float _spinSpeedMultiplier = 1;
    public float SpinSpeedModifier
    {
        get => _spinSpeedMultiplier;
    }

    [Space(5)]
    [Header("Sprites")]
    public Sprite previewSprite = default;
    public Sprite turretBaseSprite = default;
    public Sprite bodySprite = default;
    public Sprite gunMountSprite = default;

    // list of gun infos - gun sprite, gun position, gun rotation
    // on change gun should move 

    public GunInfo[] guns;
}
