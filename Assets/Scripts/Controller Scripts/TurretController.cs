using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the firing and turning of all turrets
/// </summary>
public class TurretController : ObjectPoolHandlerSingleton<TurretController, Turret>
{
    public static Color defaultTurretColor = new Color(46, 204, 117);

    // internal variables
    private List<Turret> _activeTurrets = new List<Turret>();

    EnemyController _enemyController;
    GunController _gunController;
    private void Start()
    {
        _enemyController = EnemyController.Instance;
        _gunController = GunController.Instance;
    }

    private void Update()
    {
        for (int i = 0; i < _activeTurrets.Count; i++)
        {
            var t = _activeTurrets[i];
            if (!t)
            {
                _activeTurrets.RemoveAt(i);
                continue;
            }

            // the turret is doing nothing
            if (t.state == Turret.State.none)
            {
                // try and find a target
                ChooseNewTarget(t, _enemyController.enemies);
            }

            // in any other state, we need to be targeting the enemy
            if (t.state != Turret.State.none)
            {
                TurnTurret(t);
            }
        }
    }

    public Turret PlaceTurret(Vector2 location)
    {
        // create and activate a turret at the location specified
        Turret newTurret = CreateObject(location, Angle.zero, transform);
        UpdateTurret(newTurret, newTurret.info);
        //UpdateTurretStats(newTurret.stats, newTurret.info);
        newTurret.Activate();

        // add it to our list, so we have control over it
        _activeTurrets.Add(newTurret);

        return newTurret;
    }

    public void SellTurret(Turret t)
    {
        // first deactivate the guns
        while (t.guns.Count > 0)
        {
            _gunController.DeactivateObject(t.guns[0]);
            t.guns.RemoveAt(0);
        }

        // then deactivate the turret
        DeactivateObject(t);
        _activeTurrets.Remove(t);
    }

    // updates the given turret to match the info provided
    public void UpdateTurret(Turret t, TurretInfo newInfo)
    {
        // reset targeting
        t.ResetTargeting();

        // update sprites
        UpdateTurretSprites(t, newInfo);

        // update stats
        UpdateTurretStats(t.stats, newInfo);

        // update guns
        UpdateTurretGuns(t, newInfo);

        // reset firing routine to account for gun changes
        t.RestartFiringRoutine();
    }

    private void UpdateTurretSprites(Turret t, TurretInfo newInfo)
    {
        t.turretBase.sprite = newInfo.turretBaseSprite;
        t.body.sprite = newInfo.bodySprite;
        t.gunMount.sprite = newInfo.gunMountSprite;
    }

    private void UpdateTurretGuns(Turret t, TurretInfo newInfo)
    {
        int currentGunAmount = t.guns.Count;

        int g;
        for (g = 0; g < newInfo.guns.Length; g++)
        {
            GunInfo info = newInfo.guns[g];

            if (g < currentGunAmount)
            {
                // there is an existing gun, update the existing gun to match
                _gunController.UpdateGun(t.guns[g], info, t.stats.reloadSpeed);
            }
            else
            {
                // we need to add an extra gun
                Gun newGun = _gunController.CreateGun(t.top, info, t.stats.reloadSpeed);
                newGun.turret = t;
                t.guns.Add(newGun);

                currentGunAmount++;
            }
        }

        // destroy any extra guns on the turret
        t.guns.RemoveRange(g, currentGunAmount - g);
    }



    private void UpdateTurretStats(TurretStatistics stats, TurretInfo newInfo)
    {
        if (newInfo.type == TurretInfo.Type.multiplier)
        {
            UpdateTurretStatsMultiplier(stats, newInfo);
        }
        else
        {
            UpdateTurretStatsBasic(stats, newInfo);
        }
    }
    private void UpdateTurretStatsMultiplier(TurretStatistics stats, TurretInfo newInfo)
    {
        // multiply all basic stats by modifier
        stats.damage = Mathf.RoundToInt(stats.damage * newInfo.DamageModifier);
        stats.range *= newInfo.RangeModifier;
        stats.reloadSpeed *= newInfo.ReloadSpeedModifier;
        stats.spinSpeed = Mathf.RoundToInt(stats.spinSpeed * newInfo.SpinSpeedModifier);
    }
    private void UpdateTurretStatsBasic(TurretStatistics stats, TurretInfo newInfo)
    {
        // force sets stats to whatever the modifiers are, NEGATING any augments that were applied.
        // as such, should only be used for the basic turret, before any augments.
        stats.damage = Mathf.RoundToInt(newInfo.DamageModifier);
        stats.range = newInfo.RangeModifier;
        stats.reloadSpeed = newInfo.ReloadSpeedModifier;
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
            t.state = Turret.State.none;
            return;
        }

        bool firing = t.state == Turret.State.firing;
        if (!firing)
        {
            // since we are not already firing, stop targeting enemies that go out of range
            Vector2 targetPos = t.currentTarget.Body.position;
            float targetDistance = Vector2.Distance(t.transform.position, targetPos);
            if (targetDistance > t.info.RangeModifier)
            {
                // recalculate target next frame
                t.state = Turret.State.none;
                return;
            }
        }

        Angle desiredAngle = FindAngleToTarget(t);

        // rotate towards the desired rotation by the spin speed
        t.top.rotation = Quaternion.RotateTowards
        (
            t.top.rotation, // from
            desiredAngle.AsQuaternion(), // to
            t.info.SpinSpeedModifier * Time.deltaTime // delta speed
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
        Vector2 targetPos = t.currentTarget.Body.position;
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
            return Vector2.Distance(t.transform.position, x.Body.position) <= t.info.RangeModifier;
        });

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
            // we have a target, set everything
            t.currentTarget = newTarget;
            t.currentGunIndex = newGunIndex;
            t.state = Turret.State.aiming;
        }
    }

    /// <summary>
    /// chooses a new gun and a target from a list of targets
    /// </summary>
    public void ChooseClosestTarget(Turret t, List<Enemy> targets, out Enemy newTarget, out int newGunIndex)
    {
        bool allowReloadingGuns = false;
        if (!t.guns.Find(x => x.canFire))
        {
            // if no guns are ready to fire we can default to any gun
            allowReloadingGuns = true;
        }

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
            for (int g = 0; g < t.guns.Count; g++)
            {
                Gun gun = t.guns[g];
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
    }
}