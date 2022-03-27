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
            if (t.turretState == Turret.TurretState.none)
            {
                // try and find a target
                t.ChooseNewTarget(_enemyController.enemies);
            }

            // in any other state, we need to be targeting the enemy
            if (t.turretState != Turret.TurretState.none)
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

        // temp - draw targeting lines
        //Debug.DrawLine(t.Guns[t.currentGunIndex].transform.position, targetPos);
        Debug.DrawLine(t.transform.position, targetPos);

        // between the angle from turret and gun
        Angle desiredAngle = angleFromTurret;

        return desiredAngle;
    }

    private void TurnTurret(Turret t)
    {
        if (!t.currentTarget)
        {
            // no target, recalculate next frame
            t.turretState = Turret.TurretState.none;
            return;
        }

        bool firing = t.turretState == Turret.TurretState.firing;
        if (!firing)
        {
            // since we are not already firing, stop targeting enemies that go out of range
            Vector2 targetPos = t.currentTarget.Body.position;
            float targetDistance = Vector2.Distance(t.transform.position, targetPos);
            if (targetDistance > t.Info.Range)
            {
                // recalculate target next frame
                t.turretState = Turret.TurretState.none;
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

            t.turretState = lockedToTarget ? Turret.TurretState.locked : Turret.TurretState.aiming;
        }
    }
}
