using System;
using UnityEngine;

/// <summary>
/// script for each individual gun on a turret
/// </summary>
public class Gun : MonoBehaviour
{
    static readonly string fireTrigger = "fire";
    static readonly string reloadFloat = "reload_multiplier";

    Animator animator;
    Turret turret;

    bool canFire = true;

    /// <summary>
    /// initializes the gun with all the needed info
    /// </summary>
    /// <param name="reloadTime">the reload time in seconds</param>
    public void Initialize(Turret turret, float reloadTime)
    {
        this.turret = turret;
        animator = GetComponent<Animator>();
        animator.SetFloat(reloadFloat, 1 / reloadTime);
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
            animator.SetTrigger(fireTrigger);
            return true;
        }
        return false;
    }

    // called by the animator
    public void OnAnimatorFire()
    {
        turret.HitEnemy();
    }
    public void OnAnimatorReload()
    {
        canFire = true;
    }
}
