using UnityEngine;

/// <summary>
/// holds all the *current* stats of one turret, including any modifications made by augments
/// </summary>
[System.Serializable]
public class TurretStatistics
{
    [Min(0)] public int damage;
    [Min(0)] public float range; // tiles
    [Min(0)] public int spinSpeed; // degrees/sec
    [Min(0)] public float reloadTime;

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

    public TurretStatistics()
    {
        Reset();
    }

    public void Reset()
    {
        // completely reset, everything to zero
        damage = 0;
        range = 0;
        spinSpeed = 0;
        reloadTime = 0;

        xp = 0;

        armorPiercing = 0;
        critChance = new Chance(0);
    }
}
