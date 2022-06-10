using UnityEngine;

public class AugmentationController : Singleton<AugmentationController>
{
    readonly private HSVColor baseColor = new HSVColor(145, 77, 80);

    // what does this need to do
    // apply augments to a turret
    // hold a reference of all augments
    // 
    private void Start()
    {

    }

    private void Update()
    {

    }

    public void UpdateTurretAugmentations(Turret t)
    {
        // first reset the stats
        t.statistics.Reset(t.info);

        // color handling
        HSVColor color = baseColor;
        bool isNewColor = false;

        // then iterate through the turret augments
        foreach (AugmentationInfo info in t.Augmentations)
        {
            Augmentation aug = info.augmentation;
            if (!aug)
            {
                // no augment in this slot
                continue;
            }

            // change the color here
            HSVColor augColor = HSVColor.FromColor(aug.color);
            if (!isNewColor)
            {
                isNewColor = true;
                color = augColor;
            }
            else
            {
                color = HSVColor.Lerp(color, augColor, 0.5f);
            }

            // for now just apply modifiers
            for (int i = 0; i < info.currentTier; i++)
            {
                AugmentationTier tier = aug.tiers[i];

                // now apply each modifier seperately
                if (tier.damageModifier.active)
                {
                    // damage
                    t.statistics.damage = Mathf.RoundToInt(
                        ApplyModifier(t.statistics.damage, tier.damageModifier)
                    );
                    Debug.Log("applied damage");
                }
                if (tier.spinSpeedModifier.active)
                {
                    // spin speed
                    t.statistics.spinSpeed = Mathf.RoundToInt(
                        ApplyModifier(t.statistics.spinSpeed, tier.spinSpeedModifier)
                    );
                }
                if (tier.reloadSpeedModifier.active)
                {
                    // reload speed
                    t.statistics.reloadSpeed = Mathf.RoundToInt(
                        ApplyModifier(t.statistics.reloadSpeed, tier.reloadSpeedModifier)
                    );
                }
            }
        }

        t.body.color = color.AsColor();
    }

    private float ApplyModifier(float original, AugmentationModifier modifier)
    {
        float result = modifier.multiplier
            ? original * modifier.amount
            : original + modifier.amount;
        return result;
    }
}
