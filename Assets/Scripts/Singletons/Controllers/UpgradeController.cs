using UnityEngine;

public class UpgradeController : Singleton<UpgradeController>
{

    public UpgradeInfo[] upgrades;

    public void ApplyUpgrade(TurretStatistics stats, string upgradeId)
    {
        // branch off into seperate methods for each augment type
        switch (upgradeId)
        {
            case "damage":
                ApplyDamageUpgrade(stats);
                break;
            case "range":
                ApplyRangeUpgrade(stats);
                break;
            case "spin_speed":
                ApplySpinSpeedUpgrade(stats);
                break;
            default:
                Debug.LogError($"Invalid upgrade id '{upgradeId}', please make sure your id is valid");
                break;
        }
    }

    private void ApplyDamageUpgrade(TurretStatistics stats)
    {
        stats.damage = Mathf.RoundToInt(stats.damage * 1.25f);
    }

    private void ApplyRangeUpgrade(TurretStatistics stats)
    {
        stats.range *= 1.35f;
    }
    private void ApplySpinSpeedUpgrade(TurretStatistics stats)
    {
        stats.spinSpeed = Mathf.RoundToInt(stats.spinSpeed * 1.4f);
    }
}
