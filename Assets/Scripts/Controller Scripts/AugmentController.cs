using UnityEngine;

public class AugmentController : Singleton<AugmentController>
{
    readonly private HSVColor baseColor = new HSVColor(145, 77, 80);

    [SerializeField] Augment damageAugment;

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

    public Augment ChooseNewAugmentation(Augment current)
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

    public void ApplyAugment(TurretStatistics stats, Augment.Type type, int tier)
    {
        // branch off into seperate methods for each augment type
        switch (type)
        {
            case Augment.Type.damage:
                ApplyDamageAugment(stats, tier);
                break;
            case Augment.Type.range:
                ApplyRangeAugment(stats, tier);
                break;
            default:
                // well tbh I have no idea
                break;
        }
    }

    private void ApplyDamageAugment(TurretStatistics stats, int tier)
    {
        // do stuff, switch by tier
    }
    private void ApplyRangeAugment(TurretStatistics stats, int tier)
    {
        // do stuff, switch by tier
    }

    //public void UpdateTurretAugmentations(Turret t)
    //{
    //    // first reset the stats
    //    t.stats.Reset(t.info);
    //
    //    // then iterate through the turret augments
    //    AugmentationInfo info = t.customAugment;
    //
    //    Augment aug = info.augmentation;
    //    if (!aug)
    //    {
    //        // no augment, set default color
    //        t.body.color = baseColor.AsColor();
    //        return;
    //    }
    //
    //    t.body.color = aug.color;
    //
    //    // for now just apply modifiers
    //    for (int i = 0; i < info.currentTier; i++)
    //    {
    //        AugmentationTier tier = aug.tiers[i];
    //
    //        // now apply each modifier seperately
    //        if (tier.damageModifier.active)
    //        {
    //            // damage
    //            t.stats.damage = Mathf.RoundToInt(
    //                ApplyModifier(t.stats.damage, tier.damageModifier)
    //            );
    //            Debug.Log("applied damage");
    //        }
    //        if (tier.spinSpeedModifier.active)
    //        {
    //            // spin speed
    //            t.stats.spinSpeed = Mathf.RoundToInt(
    //                ApplyModifier(t.stats.spinSpeed, tier.spinSpeedModifier)
    //            );
    //        }
    //        if (tier.reloadSpeedModifier.active)
    //        {
    //            // reload speed
    //            t.stats.reloadSpeed = Mathf.RoundToInt(
    //                ApplyModifier(t.stats.reloadSpeed, tier.reloadSpeedModifier)
    //            );
    //        }
    //    }
    //}
    //
    //private float ApplyModifier(float original, AugmentationModifier modifier)
    //{
    //    float result = modifier.multiplier
    //        ? original * modifier.amount
    //        : original + modifier.amount;
    //    return result;
    //}
}
