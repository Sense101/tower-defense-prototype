using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the firing and turning of all turrets
/// </summary>
public class TurretController : Singleton<TurretController>
{
    public readonly List<Turret> _turrets = new List<Turret>();

    EnemyController _enemyController;
    private void Start()
    {
        _enemyController = EnemyController.Instance;
    }

    private void Update()
    {
        for (int i = 0; i < _turrets.Count; i++)
        {
            var t = _turrets[i];
            if (!t)
            {
                _turrets.RemoveAt(i);
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

    private Angle FindAngleToTarget(Turret t)
    {
        Vector2 targetPos = t.currentTarget.Body.position;
        Gun currentGun = t.Guns[t.currentGunIndex];
        float localGunRotation = currentGun.transform.localEulerAngles.z;


        // from gun to target
        Angle angleFromGun = Angle.Towards(t.Guns[t.currentGunIndex].transform.position, targetPos);
        // from turret to target
        Angle angleFromTurret = Angle.Towards(t.transform.position, targetPos).Rotate(localGunRotation);

        // temp - draw targeting line
        Debug.DrawLine(t.Guns[t.currentGunIndex].transform.position, targetPos);

        // between the angle from turret and gun
        Angle desiredAngle = angleFromTurret;

        return desiredAngle;
    }

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
            if (targetDistance > t.Info.Range)
            {
                // recalculate target next frame
                t.state = Turret.State.none;
                return;
            }
        }

        Angle desiredAngle = FindAngleToTarget(t);

        // rotate towards the desired rotation by the spin speed
        t.Top.rotation = Quaternion.RotateTowards
        (
            t.Top.rotation, // from
            desiredAngle.AsQuaternion(), // to
            t.Info.SpinSpeed * Time.deltaTime // delta speed
        );


        // set the turret state, but only if we are not already firing
        if (!firing)
        {
            // work out the difference between our current rotation and target rotation
            float currentDiff = t.Top.transform.eulerAngles.z - desiredAngle.degrees;
            bool lockedToTarget = currentDiff == 0;

            t.state = lockedToTarget ? Turret.State.locked : Turret.State.aiming;
        }
    }

    private void ChooseNewTarget(Turret t, List<Enemy> enemies)
    {
        // find all the targets in range
        List<Enemy> targetsInRange = enemies.FindAll(x => {
            return Vector2.Distance(t.transform.position, x.Body.position) <= t.Info.Range;
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
        if (!t.Guns.Find(x => x.canFire))
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
                newGunIndex = gunIndex;
            }
        }
    }
}