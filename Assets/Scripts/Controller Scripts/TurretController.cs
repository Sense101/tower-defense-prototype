using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// controls the firing and turning of all turrets
/// </summary>
public class TurretController : ObjectPoolHandlerSingleton<TurretController, Turret>
{
    public TurretInfo basicInfo;

    EnemyController _enemyController;
    GunController _gunController;
    private void Start()
    {
        _enemyController = EnemyController.Instance;
        _gunController = GunController.Instance;
    }

    private void Update()
    {
        foreach (Turret t in objects)
        {
            switch (t.state)
            {
                case Turret.State.lookingForTarget:
                    // the turret wants a target, give it one
                    ChooseNewTarget(t, _enemyController.activeEnemies);
                    break;
                case Turret.State.aiming:
                case Turret.State.locked:
                case Turret.State.firing:
                    // turn towards the target
                    TurnTurret(t);
                    break;
                default:
                    // do nothing
                    break;
            }
        }
    }

    public Turret PlaceTurret(Vector2 location)
    {
        // create and activate a turret at the location specified
        Turret newTurret = CreateObject(location, Angle.zero, transform);
        ModifyTurret(newTurret, newTurret.info);

        newTurret.Activate();

        return newTurret;
    }

    public void SellTurret(Turret t)
    {
        // first deactivate the guns
        while (t.guns.Count > 0)
        {
            _gunController.DeactivateGun(t.guns[0]);
        }

        // then deactivate the turret
        DeactivateObject(t);

        // set the info back to basic
        t.info = basicInfo;
    }

    // modifies the given turret to match the info provided
    public void ModifyTurret(Turret t, TurretInfo newInfo)
    {
        // reset targeting
        t.StopFiringRoutine();
        t.state = Turret.State.lookingForTarget;
        t.currentGunIndex = 0;

        // modify sprites
        ModifyTurretSprites(t, newInfo);

        // modify stats
        ModifyTurretStats(t.stats, newInfo);

        // modify guns - @TODO this needs to completely reset all guns, and it doesn't right now
        ModifyTurretGuns(t, newInfo);

        // start firing again
        t.StartFiringRoutine();

        t.info = newInfo;
    }

    private void ModifyTurretSprites(Turret t, TurretInfo newInfo)
    {
        t.turretBase.sprite = newInfo.turretBaseSprite;
        t.body.sprite = newInfo.bodySprite;
        t.gunMount.sprite = newInfo.gunMountSprite;
    }

    private void ModifyTurretGuns(Turret t, TurretInfo newInfo)
    {
        int currentGunAmount = t.guns.Count;

        int g;
        for (g = 0; g < newInfo.guns.Length; g++)
        {
            GunInfo info = newInfo.guns[g];

            if (g < currentGunAmount)
            {
                // there is an existing gun, modify the existing gun to match
                _gunController.ModifyGun(t.guns[g], info, t);
            }
            else
            {
                // we need to add an extra gun
                Gun newGun = _gunController.CreateGun(t.top, info, t);
                currentGunAmount++;
            }
        }

        // remove any extra guns on the turret
        List<Gun> extraGuns = t.guns.GetRange(g, currentGunAmount - g);
        foreach (Gun gun in extraGuns)
        {
            _gunController.DeactivateGun(gun);
        }
    }



    private void ModifyTurretStats(TurretStatistics stats, TurretInfo newInfo)
    {
        if (newInfo.type == TurretInfo.Type.multiplier)
        {
            ModifyTurretStatsMultiplier(stats, newInfo);
        }
        else
        {
            ModifyTurretStatsBasic(stats, newInfo);
        }
    }
    private void ModifyTurretStatsMultiplier(TurretStatistics stats, TurretInfo newInfo)
    {
        // multiply all basic stats by modifier
        stats.damage = Mathf.RoundToInt(stats.damage * newInfo.DamageModifier);
        stats.range *= newInfo.RangeModifier;
        stats.reloadTime *= newInfo.ReloadTimeModifier;
        stats.spinSpeed = Mathf.RoundToInt(stats.spinSpeed * newInfo.SpinSpeedModifier);
    }
    private void ModifyTurretStatsBasic(TurretStatistics stats, TurretInfo newInfo)
    {
        // force sets stats to whatever the modifiers are, NEGATING any augments that were applied.
        // as such, should only be used for the basic turret, before any augments.
        stats.damage = Mathf.RoundToInt(newInfo.DamageModifier);
        stats.range = newInfo.RangeModifier;
        stats.reloadTime = newInfo.ReloadTimeModifier;
        stats.spinSpeed = Mathf.RoundToInt(newInfo.SpinSpeedModifier);
    }

    // --------------------------------
    // moving and aiming turrets
    // --------------------------------

    private void TurnTurret(Turret t)
    {
        if (!t.currentTarget)
        {
            // no target, recalculate next frame
            t.state = Turret.State.lookingForTarget;
            return;
        }

        bool firing = t.state == Turret.State.firing;
        if (!firing)
        {
            // since we are not already firing, stop targeting enemies that go out of range
            Vector2 targetPos = t.currentTarget.body.transform.position;
            float targetDistance = Vector2.Distance(t.transform.position, targetPos);
            if (targetDistance > t.stats.range)
            {
                // recalculate target next frame
                t.state = Turret.State.lookingForTarget;
                return;
            }
        }

        Angle desiredAngle = FindAngleToTarget(t);

        // rotate towards the desired rotation by the spin speed
        t.top.rotation = Quaternion.RotateTowards
        (
            t.top.rotation, // from
            desiredAngle.AsQuaternion(), // to
            t.stats.spinSpeed * Time.deltaTime // delta speed
        );


        // set the turret state, but only if we are not already firing
        if (!firing)
        {
            // work out the difference between our current rotation and target rotation
            float currentDiff = t.top.transform.eulerAngles.z - desiredAngle.degrees;
            bool lockedToTarget = currentDiff == 0;

            t.state = lockedToTarget ? Turret.State.locked : Turret.State.aiming;
        }
    }

    private Angle FindAngleToTarget(Turret t)
    {
        Vector2 targetPos = t.currentTarget.body.transform.position;
        Gun currentGun = t.guns[t.currentGunIndex];
        float localGunRotation = currentGun.transform.localEulerAngles.z;


        // from gun to target
        Angle angleFromGun = Angle.Towards(t.guns[t.currentGunIndex].transform.position, targetPos);
        // from turret to target
        Angle angleFromTurret = Angle.Towards(t.transform.position, targetPos).Rotate(localGunRotation);

        // temp - draw targeting line
        Debug.DrawLine(t.guns[t.currentGunIndex].transform.position, targetPos);

        // between the angle from turret and gun
        Angle desiredAngle = angleFromTurret;

        return desiredAngle;
    }

    private void ChooseNewTarget(Turret t, List<Enemy> enemies)
    {
        // find all the targets in range
        List<Enemy> targetsInRange = enemies.FindAll(x => {
            return Vector2.Distance(t.transform.position, x.body.transform.position) <= t.stats.range;
        });

        if (targetsInRange.Count < 1)
        {
            // nothing in range
            return;
        }

        List<Enemy> finalTargets = new List<Enemy>();

        switch (t.targetType)
        {
            case Turret.TargetType.close:
            case Turret.TargetType.first: //@TODO this is temporary, not implemented yet
                {
                    finalTargets = targetsInRange;
                    break;
                }
            case Turret.TargetType.strong:
                {
                    // filter by the most health
                    int mostHealth = 0;
                    foreach (Enemy target in targetsInRange)
                    {
                        if (target.currentHealth > mostHealth)
                        {
                            mostHealth = target.currentHealth;
                        }
                    }

                    finalTargets = targetsInRange.FindAll(x => x.currentHealth == mostHealth);
                    break;
                }
            case Turret.TargetType.weak:
                {
                    // filter by the least health
                    int leastHealth = 100;
                    foreach (Enemy target in targetsInRange)
                    {
                        if (target.currentHealth < leastHealth)
                        {
                            leastHealth = target.currentHealth;
                        }
                    }

                    finalTargets = targetsInRange.FindAll(x => x.currentHealth == leastHealth);
                    break;
                }
            default:
                {
                    Debug.LogError("Unknown turret target type: " + t.targetType);
                    break;
                }
        }

        // decide whether we use closest or special
        // for closest, just do the standard
        // for strong, find the highest health, and choose closest of all those
        // for weak, find weakest, and closest
        Enemy newTarget;
        int newGunIndex;
        ChooseClosestTarget(t, finalTargets, out newTarget, out newGunIndex);

        if (newTarget)
        {
            // we have a target, set everything and start turning the same frame
            t.currentTarget = newTarget;
            t.currentGunIndex = newGunIndex;
            t.state = Turret.State.aiming;
            TurnTurret(t);
        }
    }

    /// <summary>
    /// chooses a new gun and a target from a list of targets
    /// </summary>
    public void ChooseClosestTarget(Turret t, List<Enemy> targets, out Enemy newTarget, out int newGunIndex)
    {
        newTarget = null;
        newGunIndex = 0;

        // cycle through available targets and find the closest to a gun
        float newTargetDistance = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            Enemy target = targets[i];
            if (!target.IsSpaceForDamage())
            {
                // don't bother, it's going to die
                continue;
            }

            // find the closest gun to the enemy
            float distanceToGun = 0;
            int gunIndex = 0;

            // filter our list of guns by the ones with the lowest reload time, and then by distance
            float lowestReloadTime = t.guns.Min(x => x.remainingReloadTime);

            int gunAmount = t.guns.Count;
            for (int g = 0; g < gunAmount; g++)
            {
                Gun gun = t.guns[g];
                if (gun.remainingReloadTime > lowestReloadTime)
                {
                    continue;
                }

                float targetDistance = Vector2.Distance(gun.transform.position, target.body.transform.position);
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
    }
}