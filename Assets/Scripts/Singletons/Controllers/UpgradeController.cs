using UnityEngine;

public class UpgradeController : Singleton<UpgradeController>
{
    readonly private HSVColor baseColor = new HSVColor(145, 77, 80);

    public UpgradeInfo damageUpgrade;
    public UpgradeInfo rangeUpgrade;

    // what does this need to do
    // apply augments to a turret
    // hold a reference of all augments
    // 

    public void ApplyUpgrade(TurretStatistics stats, string upgradeId, int tier)
    {
        // branch off into seperate methods for each augment type
        switch (upgradeId)
        {
            case "damage":
                ApplyDamageUpgrade(stats, tier);
                break;
            case "range":
                ApplyRangeUpgrade(stats, tier);
                break;
            default:
                // well tbh I have no idea
                break;
        }
    }

    private void ApplyDamageUpgrade(TurretStatistics stats, int tier)
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
    private void ApplyRangeUpgrade(TurretStatistics stats, int tier)
    {
        // do stuff, switch by tier
        //temp
        stats.range *= 1.2f;
    }
}
