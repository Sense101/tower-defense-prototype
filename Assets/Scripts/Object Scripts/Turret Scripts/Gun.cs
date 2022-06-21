using Animancer;
using UnityEngine;

/// <summary>
/// script for each individual gun on a turret
/// </summary>
public class Gun : PoolObject
{
    const string FIRE_TRIGGER = "fire";
    const string RESET_TRIGGER = "reset";
    const string ACTIVATE_TRIGGER = "activate";
    const string DEACTIVATE_TRIGGER = "deactivate";
    const string RELOAD_MULTIPLIER = "reload_multiplier";

    Animator _animator;
    public Turret turret;

    public bool canFire = false;
    private bool active = false;

    public override void SetReferences()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        active = true;
        canFire = true;
        _animator.SetTrigger(ACTIVATE_TRIGGER);
    }

    public override void Deactivate()
    {
        // prevent firing and hide
        active = false;
        _animator.SetTrigger(DEACTIVATE_TRIGGER);
    }

    public override void Reset()
    {
        // resets the gun back to its base state
        _animator.SetTrigger(RESET_TRIGGER);
    }

    public void SetAnimatorController(RuntimeAnimatorController newController)
    {
        _animator.runtimeAnimatorController = newController;
    }

    public void SetReloadSpeed(float reloadSpeed)
    {
        // set speed of reload animation
        _animator.SetFloat(RELOAD_MULTIPLIER, 1 / reloadSpeed);
    }

    /// <summary>
    /// Tries to fire the gun
    /// </summary>
    /// <returns>if the gun fired succesfully</returns>
    public bool TryFire()
    {
        if (active && canFire)
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
        // the firing animation is finished, hit the enemy now
        turret.HitEnemy();
    }

    public void OnAnimatorReload()
    {
        canFire = true;
        Debug.Log("reload");
    }
}
