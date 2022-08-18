using UnityEngine;

/// <summary>
/// Instance of each enemy
/// </summary>
public class Enemy : PoolObject
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

    //@TODO this only works if there is one entrance for enemies
    public float distanceWalked = 0;

    public void SetReferences(Camera mainCamera)
    {
        healthBar.SetReferences(mainCamera);
    }

    public override void Activate()
    {
        // set health
        previewDamage = 0;
        currentHealth = info.Health;

        distanceWalked = 0;
    }

    public override void Deactivate()
    {
        body.sprite = null;
        healthBar.Hide();
    }

    public override void SetRotation(Angle rotation)
    {
        // the enemy itself should never turn around
        body.transform.rotation = rotation.AsQuaternion();
    }

    // does this enemy have room for more pain?
    public bool IsSpaceForDamage()
    {
        return currentHealth - previewDamage > 0;
    }

    /// <summary>
    /// reduces the enemies health/armor
    /// </summary>
    /// <returns>@todo the reduction of health and armor</returns>
    public void TakeHit(int damage, int armorPiercing)
    {
        //@TODO implement armor
        var armor = Mathf.Max(0, info.Armor - armorPiercing);
        var finalDamage = damage - armor;

        currentHealth -= finalDamage;

        healthBar.SetFill((float)currentHealth / info.Health);
    }
}