using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour
{
    public enum State { none, aiming, locked, firing }
    public enum TargetType { close, first, strong, weak }

    // set in inspector
    public TurretInfo Info;
    public Transform Top = default; // the part of the turret that rotates
    public List<Gun> Guns = new List<Gun>();


    // info unique to this turret
    public Enemy currentTarget = null;
    public int currentGunIndex = 0;
    public State state = State.none;
    public TargetType targetType = TargetType.first;

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

    public virtual IEnumerator FireGuns()
    {
        var gunCount = Guns.Count;
        var reloadSpeed = Info.ReloadSpeed;
        while (true)
        {
            // wait till we are locked onto the turret
            yield return new WaitUntil(() => state == State.locked);

            bool spaceForDamage = currentTarget.currentHealth - currentTarget.previewDamage > 0;
            if (spaceForDamage && Guns[currentGunIndex].TryFire())
            {
                state = State.firing;

                // add preview
                currentTarget.previewDamage += Info.Damage;

                // wait till we are done firing the current gun
                yield return new WaitUntil(() => state != State.firing);

                // @TODO don't just cycle through, find the most eligible gun
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
        state = State.none;

        // make sure the enemy exists
        if (currentTarget)
        {
            // remove preview
            currentTarget.previewDamage -= Info.Damage;

            currentTarget.TakeHit(Info.Damage, 0);

        }
        currentTarget = null;
    }

    /// <summary>
    /// chooses a new gun and a target from a list of targets
    /// </summary>
    public void ChooseClosestTarget(Turret t, List<Enemy> targetsInRange, out Enemy newTarget, out int newGun)
    {
        bool allowReloadingGuns = false;
        if (!Guns.Find(x => x.canFire))
        {
            // if no guns are ready to fire we can default to any gun
            allowReloadingGuns = true;
        }

        newTarget = null;
        newGun = 0;

        // cycle through available targets and find the closest to a gun
        float newTargetDistance = 0;
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            Enemy target = targetsInRange[i];

            // find the closest gun to the enemy
            float distanceToGun = 0;
            int gunIndex = 0;
            for (int g = 0; g < t.Guns.Count; g++)
            {
                Gun gun = t.Guns[g];
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
                newGun = gunIndex;
            }
        }

        if (newTarget)
        {
            currentTarget = newTarget;
            currentGunIndex = newGun;

            // start aiming at the target

        }
    }
}
