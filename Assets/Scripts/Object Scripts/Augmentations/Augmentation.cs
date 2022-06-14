using UnityEngine;

/// <summary>
/// this is the actual augentation scriptable object
/// </summary>
[CreateAssetMenu]
public class Augmentation : ScriptableObject
{
    public enum SpriteLocation { turret, guns }

    // the name of the augmentation
    public string title = "";

    // this augmentation's unique color - this will mix with others on a turret
    public Color color = Color.white;

    [Header("Sprite")]
    // whether to have the sprite be on the turret, or on each gun
    public SpriteLocation spriteLocation = SpriteLocation.turret;

    // offset from the center
    public Vector2 spriteOffset;
    public int rendererOrder = 10;

    [Space(5)]
    public AugmentationTier[] tiers;

    public virtual void ApplyAugment(TurretStatistics stats, int tier)
    {
        switch (tier)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                Debug.LogError("Got tier to apply that is not between 1-5!");
                break;
        }
    }
}
