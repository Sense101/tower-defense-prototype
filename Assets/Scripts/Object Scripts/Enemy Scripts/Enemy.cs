using UnityEngine;

/// <summary>
/// Instance of each enemy
/// </summary>
public class Enemy : MonoBehaviour
{

    // set in inspector
    public EnemyInfo info;
    public Transform body;
    public SpriteRenderer[] renderers;

    // set upon creation
    public HealthBar healthBar;

    // where we are headed
    public PathPoint targetPoint;
    public Vector2 targetPosition;
    public float offset;

    // health
    public int previewDamage = 0; // incoming damage that hasn't taken effect yet
    public int currentHealth = 0;
    public int currentArmor = 0;

    public virtual void EnemyStart()
    {
        // set stats
        currentHealth = info.health;
        currentArmor = info.armor;

        foreach (SpriteRenderer renderer in renderers)
        {
            // make sure the faster enemies are on top
            renderer.sortingOrder = Mathf.RoundToInt(info.moveSpeed);
        }
    }

    // does this enemy have room for more pain?
    public bool IsSpaceForDamage()
    {
        int effectiveArmor = currentArmor * (1 + info.armorStrength);
        return (currentHealth + effectiveArmor) - previewDamage > 0;
    }

    public virtual void SetBodyRotation(Angle rotation)
    {
        body.transform.rotation = rotation.AsQuaternion();
    }

    /// <summary>
    /// reduces the enemies health/armor
    /// </summary>
    /// <returns>the xp that the turret gets</returns>
    public virtual int TakeHit(int damage, int armorPiercing)
    {
        // check to seei f we have armor first
        if (currentArmor > 0)
        {
            // damage armor
            int damageReduction = Mathf.Max(0, info.armorStrength - armorPiercing);
            int finalDamage = Mathf.Max(0, damage - damageReduction);

            currentArmor = Mathf.Max(0, currentArmor - finalDamage);

            // set the health bar fill
            healthBar.SetFill(1, (float)currentArmor / info.armor);

            // turrets get no xp for destroying armor
            return 0;
        }

        currentHealth -= damage;
        int extraDamage = Mathf.Max(0, -currentHealth);

        if (currentHealth <= 0)
        {
            // we are dead, let us die
            Destroy(gameObject);
            CoinController.Instance.AddCoins(info.killReward);
        }

        // set the health bar fill
        healthBar.SetFill((float)currentHealth / info.health, 0);

        return damage - extraDamage;



    }
}