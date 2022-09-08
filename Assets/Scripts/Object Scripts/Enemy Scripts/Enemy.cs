using UnityEngine;

/// <summary>
/// Instance of each enemy
/// </summary>
public class Enemy : MonoBehaviour
{

    // set in inspector
    public EnemyInfo info;
    public SpriteRenderer body;
    public HealthBar healthBar;

    // where we are headed
    public PathPoint targetPoint;

    // health
    public int previewDamage = 0; // damage that hasn't taken effect yet
    public int currentHealth = 0; // actual health

    public virtual void SetReferences(Camera mainCamera)
    {
        healthBar.SetReferences(mainCamera);
    }

    public virtual void EnemyStart()
    {
        // set health
        currentHealth = info.health;
    }

    // does this enemy have room for more pain?
    public bool IsSpaceForDamage()
    {
        return currentHealth - previewDamage > 0;
    }

    public virtual void SetBodyRotation(Angle rotation)
    {
        body.transform.rotation = rotation.AsQuaternion();
    }

    /// <summary>
    /// reduces the enemies health/armor
    /// </summary>
    /// <returns>@todo the reduction of health and armor</returns>
    public virtual void TakeHit(int damage, int armorPiercing)
    {
        //@TODO implement armor
        var armor = Mathf.Max(0, info.armor - armorPiercing);
        var finalDamage = damage - armor;

        currentHealth -= finalDamage;

        healthBar.SetFill((float)currentHealth / info.health);

        if (currentHealth <= 0)
        {
            // we are dead, let us die
            Destroy(gameObject);
            CoinController.Instance.AddCoins(info.killReward);
        }
    }
}