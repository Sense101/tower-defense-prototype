using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class TurretInfo : ScriptableObject
{
    //@TODO maybe put damage/reload on the gun?

    // Damage
    [Tooltip("the damage the turret does")]
    [SerializeField] int _damage = 10;
    public int Damage
    {
        get => _damage;
    }


    // Range
    [Tooltip("the range of the turret in tiles")]
    [SerializeField] float _range = 1;
    public float Range
    {
        get
        {
            // add on 0.5 to exclude the tile the turret is on
            return _range + 0.5f;
        }
    }

    // Reload Speed
    [Tooltip("the time it takes for a gun to reload in seconds")]
    [Range(0.1f, 10)]
    [SerializeField] float _reloadSpeed = 1;
    public float ReloadSpeed
    {
        get => _reloadSpeed;
    }

    // Spin Speed
    [Tooltip("spin speed in degrees per second")]
    [Range(1, 360)]
    [SerializeField] int _spinSpeed = 90;
    public int SpinSpeed
    {
        get => _spinSpeed;
    }

    [Space(5)]
    [Header("Sprites")]
    public Sprite fullSprite = default;
    public Sprite baseSprite = default;
    public Sprite previewSprite = default;
    //@TODO add all sprites for dynamic turret upgrading?
}
