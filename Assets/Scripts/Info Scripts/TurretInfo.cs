using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Infos/TurretInfo")]
public class TurretInfo : ScriptableObject
{
    // the turret type, controls what it does to the turret stats
    public enum Type { multiplier, basic }

    public Type type = Type.multiplier;

    public int cost = 0;

    public string title = "";
    [TextArea(3, 10)] public string description = "";

    [Space(5)]

    [Tooltip("the damage the turret does")]
    public float damageModifier = 1;

    [Tooltip("the range of the turret in tiles")]
    public float rangeModifier = 1;

    [Tooltip("the time it takes for a gun to reload in seconds")]
    public float reloadTimeModifier = 1;

    [Tooltip("spin speed in degrees per second")]
    public float spinSpeedModifier = 1;

    [Space(5)]
    [Header("Sprites")]
    public Sprite fullSprite = default; // the full sprite, including the base
    public Sprite previewSprite = default; // without the base
    public Sprite turretBaseSprite = default; // just the base
    public Sprite bodySprite = default; // the body
    public Sprite gunMountSprite = default; // just the gun mount

    [Space(5)]
    public GunInfo[] guns; // @todo replace gun system with instantiating new guns for more freedom?

    [Space(5)]
    public BulletInfo bulletInfo; // if the above is done no need to have this here!

    [Space(5)]
    public TurretInfo[] mutationPaths;
}
