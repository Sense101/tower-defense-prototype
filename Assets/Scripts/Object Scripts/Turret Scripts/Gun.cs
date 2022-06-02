using UnityEngine;

/// <summary>
/// script for each individual gun on a turret
/// </summary>
public class Gun : MonoBehaviour
{
    static readonly string FIRE_TRIGGER = "fire";
    static readonly string RELOAD_MULTIPLIER = "reload_multiplier";

    Animator _animator;
    Turret _turret;

    public bool canFire = true;

    /// <summary>
    /// initializes the gun with all the needed info
    /// </summary>
    /// <param name="reloadTime">the reload time in seconds</param>
    public void Initialize(Turret turret, float reloadTime)
    {
        _turret = turret;
        _animator = GetComponent<Animator>();
        _animator.SetFloat(RELOAD_MULTIPLIER, 1 / reloadTime);
    }

    /// <summary>
    /// Tries to fire the gun
    /// </summary>
    /// <returns>if the gun fired succesfully</returns>
    public bool TryFire()
    {
        if (canFire)
        {
            canFire = false;
            // fire
            _animator.SetTrigger(FIRE_TRIGGER);
            return true;
        }
        return false;
    }

    // called by the animator
    public void OnAnimatorFire()
    {
        // the firing animation is finished
        _turret.HitEnemy();
    }

    public void OnAnimatorReload()
    {
        canFire = true;
    }
}
