using UnityEngine;

// class that store all info unique to each bullet
[System.Serializable]
public class BulletStatistics
{
    public Enemy target = null;
    public int damage = 0;
    public int armorPiercing = 0;

    public BulletStatistics(Enemy target, int damage, int armorPiercing)
    {
        Modify(target, damage, armorPiercing);
    }

    public void Modify(Enemy newTarget, int newDamage, int newArmorPiercing)
    {
        target = newTarget;
        damage = newDamage;
        armorPiercing = newArmorPiercing;
    }
}
