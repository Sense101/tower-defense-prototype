using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour
{
    public enum TurretState { none, aiming, locked, firing }

    // set in inspector
    public TurretInfo Info;
    public Transform Top = default; // the part of the turret that rotates
    public List<Gun> Guns = new List<Gun>();


    // info unique to this turret
    public Enemy currentTarget = null;
    public int currentGunIndex = 0;
    public TurretState turretState = TurretState.none;

    // reload time / number of guns = wait time between shots - UNLESS OVERRIDEN

    /// <summary>
    /// initialize the turret
    /// </summary>
    public void Initialize()
    {

        for (int i = 0; i < Guns.Count; i++)
        {
            var gun = Guns[i];
            gun.Initialize(this, Info.ReloadSpeed);
        }

        StartCoroutine(FireGuns());
    }

    // @TODO currently only handles one gun correctly
    public virtual IEnumerator FireGuns()
    {
        var gunCount = Guns.Count;
        var reloadSpeed = Info.ReloadSpeed;
        while (true)
        {
            // wait till we are locked onto 
            yield return new WaitUntil(() => turretState == TurretState.locked);

            if (Guns[currentGunIndex].TryFire())
            {
                turretState = TurretState.firing;

                // wait till we are done firing the current gun
                yield return new WaitUntil(() => turretState != TurretState.firing);

                // @TODO on't just cycle through, find the most eligible gun
                // will have to calculate closest target to each?
                // move to the next gun
                //currentGunIndex = (currentGunIndex + 1) % gunCount;


                // prevent firing of next gun till we hit a nicer ratio
                yield return new WaitForSeconds(reloadSpeed / gunCount);
            }
        }
    }

    // called by gun
    public void HitEnemy()
    {
        // reset the turret
        turretState = TurretState.none;

        // the enemy may have been destroyed while we were firing
        if (currentTarget)
        {
            currentTarget.TakeHit(Info.Damage, 0);
        }
    }

    /// <summary>
    /// chooses a new gun and a target from a list of targets
    /// </summary>
    public void ChooseNewTarget(List<Enemy> targets)
    {
        bool allowReloadingGuns = false;
        if (!Guns.Find(x => x.canFire))
        {
            // if no guns are ready to fire we can default to any gun
            allowReloadingGuns = true;
        }

        // find all the targets in range
        List<Enemy> targetsInRange = targets.FindAll(x => {
            return Vector2.Distance(transform.position, x.Body.position) <= Info.Range;
        });

        Enemy newTarget = null;
        int newGunIndex = 0;

        // cycle through available targets and find the most eligible (currently just closest!)
        float newTargetDistance = 0;
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            Enemy target = targetsInRange[i];

            // find the closest gun to the enemy
            float distanceToGun = 0;
            int gunIndex = 0;
            for (int g = 0; g < Guns.Count; g++)
            {
                Gun gun = Guns[g];
                if (!gun.canFire && !allowReloadingGuns)
                {
                    continue;
                }

                float targetDistance = Vector2.Distance(gun.transform.position, target.Body.position);
                if (distanceToGun == 0 || targetDistance < distanceToGun)
                {
                    // it's closer, set it as the gun
                    distanceToGun = targetDistance;
                    gunIndex = g;
                }
            }

            // if it's closer or we have none set yet, set it as the target
            if (newTargetDistance == 0 || distanceToGun < newTargetDistance)
            {
                newTarget = target;
                newTargetDistance = distanceToGun;
                newGunIndex = gunIndex;
            }
        }

        if (newTarget)
        {
            currentTarget = newTarget;
            currentGunIndex = newGunIndex;

            // start aiming at the target
            turretState = TurretState.aiming;
        }
    }
}
