using UnityEngine;

[CreateAssetMenu]
public class Augmentation : ScriptableObject
{
    public enum SpriteLocation { turret, guns }

    // the name of the augmentation
    public string title = "";

    // this augmentation's unique color - this will mix with others on a turret
    public Color color = new Color(1, 1, 1);

    [Header("Sprite")]
    // whether to have the sprite be on the turret, or on each gun
    public SpriteLocation spriteLocation = SpriteLocation.turret;

    // offset from the center
    public Vector2 spriteOffset;
    public int rendererOrder = 10;

    [Space(5)]
    public AugmentationTierInfo[] tiers;
}
