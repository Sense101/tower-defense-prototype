using UnityEngine;

/// <summary>
/// holds all the *current* stats of one turret, including any modifications made by augments
/// </summary>
[System.Serializable]
public class TurretStatistics
{
    /// <summary>
    /// the experience the turret has
    /// </summary>
    public int xp = 0; // this will always start at 0

    /// <summary>
    /// the damage that is done to enemies
    /// </summary>
    public int damage;

    /// <summary>
    /// the turn speed in degrees/second
    /// </summary>
    public int turnSpeed;

    /// <summary>
    /// the time to reload in seconds
    /// </summary>
    public float reloadSpeed;

    /// <summary>
    /// the extra damage done to enemy armor
    /// </summary>
    public int armorPiercing = 0;

    public Chance critChance = new Chance(0);

    // unfinished

    public int slowdown; //chance/amount

    public int stun; //chance/amount

    public bool seeCloakedEnemies = false; //(temp, adds an eye on the turret)



    // armor piercing amount
    // see cloaking enemies skill?
    // crit chance (double damage)
    // poison/virus amount

    public TurretStatistics(int damage, float turnSpeed, float reloadSpeed, int armorPiercing)
    {

    }

    // lightning will be left for now
}
