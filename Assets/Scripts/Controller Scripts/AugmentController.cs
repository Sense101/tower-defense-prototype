using UnityEngine;

public class AugmentController : Singleton<AugmentController>
{
    readonly private HSVColor baseColor = new HSVColor(145, 77, 80);

    public Augment damageAugment;
    public Augment rangeAugment;

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

    public Augment ChooseNewAugment(Augment current)
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
        switch (tier)
        {
            case 1:
                stats.damage = Mathf.RoundToInt(stats.damage * 1.5f);
                break;
            case 2:
                // do a thing
                stats.damage = Mathf.RoundToInt(stats.damage * 1.5f);
                break;
            case 3:
                // do a thing
                stats.damage = Mathf.RoundToInt(stats.damage * 1.5f);
                break;
            case 4:
                // do a thing
                stats.damage = Mathf.RoundToInt(stats.damage * 1.5f);
                break;
            case 5:
                // do a thing
                stats.damage = Mathf.RoundToInt(stats.damage * 1.5f);
                break;
            default:
                // you expect me to apply a nonexistent tier?? no.
                Debug.LogError($"Tried to apply nonexistent augment tier {tier}");
                break;
        }
    }
    private void ApplyRangeAugment(TurretStatistics stats, int tier)
    {
        // do stuff, switch by tier
        stats.range = Mathf.RoundToInt(stats.range * 1.5f);
    }
}
