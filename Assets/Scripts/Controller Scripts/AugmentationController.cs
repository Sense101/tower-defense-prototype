using UnityEngine;

public class AugmentationController : Singleton<AugmentationController>
{
    readonly private HSVColor baseColor = new HSVColor(145, 77, 80);

    [SerializeField] Augmentation damageAugment;

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

    public Augmentation ChooseNewAugmentation(Augmentation current)
    {
        if (current == null)
        {
            return damageAugment;
        }
        else
        {
            return null;
        }
    }

    public void UpdateTurretAugmentations(Turret t)
    {
        // first reset the stats
        t.statistics.Reset(t.info);

        // then iterate through the turret augments
        AugmentationInfo info = t.augment;

        Augmentation aug = info.augmentation;
        if (!aug)
        {
            // no augment, set default color
            t.body.color = baseColor.AsColor();
            return;
        }

        t.body.color = aug.color;

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

    private float ApplyModifier(float original, AugmentationModifier modifier)
    {
        float result = modifier.multiplier
            ? original * modifier.amount
            : original + modifier.amount;
        return result;
    }
}
