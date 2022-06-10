using UnityEngine;

[System.Serializable]
public class AugmentationTier
{
    public int upgradeCost = 0;

    // this is shown in the interface
    public Sprite icon;

    // this is shown on the turret or each gun
    public Sprite sprite;

    [Header("Modifiers")]
    public AugmentationModifier damageModifier;
    public AugmentationModifier spinSpeedModifier;
    public AugmentationModifier reloadSpeedModifier;
}
