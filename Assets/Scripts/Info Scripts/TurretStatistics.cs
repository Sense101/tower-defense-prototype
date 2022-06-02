using UnityEngine;

/// <summary>
/// holds all the *current* stats of one turret, including any modifications made by augments
/// </summary>
[System.Serializable]
public class TurretStatistics
{
    // these three are set upon initialization
    public int damage;
    public int spinSpeed; // degrees/sec
    public float reloadSpeed;
    public TurretStatistics(int damage, int spinSpeed, float reloadSpeed)
    {
        this.damage = damage;
        this.spinSpeed = spinSpeed;
        this.reloadSpeed = reloadSpeed;
    }

    public int xp = 0;
    public int armorPiercing = 0;

    public Chance critChance = new Chance(0);

    // and damage over time

    // unfinished

    public int slowdown; //chance/amount

    public int stun; //chance/amount

    public bool seeCloakedEnemies = false; //(temp, adds an eye on the turret)

    // lightning will be left for now
}
