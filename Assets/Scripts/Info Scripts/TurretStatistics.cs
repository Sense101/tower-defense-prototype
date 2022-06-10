using UnityEngine;

/// <summary>
/// holds all the *current* stats of one turret, including any modifications made by augments
/// </summary>
[System.Serializable]
public class TurretStatistics
{
    // these three are set upon initialization
    [Min(0)] public int damage;
    [Min(0)] public int spinSpeed; // degrees/sec
    [Min(0)] public float reloadSpeed;

    [Min(0)] public int xp;
    [Min(0)] public int armorPiercing;

    public Chance critChance;

    // and damage over time
    // turret needs to store a list of augments

    // unfinished

    public int slowdown; //chance/amount

    public int stun; //chance/amount

    public bool seeCloakedEnemies = false; //(temp, adds an eye on the turret)

    // lightning will be left for now

    public TurretStatistics(TurretInfo info)
    {
        Reset(info, true);
    }

    public void Reset(TurretInfo info, bool includeXp = false)
    {
        damage = info.Damage;
        spinSpeed = info.SpinSpeed;
        reloadSpeed = info.ReloadSpeed;

        armorPiercing = 0;
        critChance = new Chance(0);

        if (includeXp)
        {
            xp = 0;
        }
    }
}
