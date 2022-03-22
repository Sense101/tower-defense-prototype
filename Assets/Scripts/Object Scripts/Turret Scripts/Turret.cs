using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // set in inspector
    public TurretInfo Info;
    [SerializeField] Transform Top = default; // the part of the turret that rotates
    public List<Gun> Guns = new List<Gun>();


    private Enemy _currentEnemy = null;
    private bool _facingTarget = false;
    public TurretInfo.FiringState _firingState = TurretInfo.FiringState.none;

    EnemyController _enemyController;


    // reload time / number of guns = wait time between shots - UNLESS OVERRIDEN

    /// <summary>
    /// initialize the turret
    /// </summary>
    public void Initialize(EnemyController enemyController)
    {
        _enemyController = enemyController;
        for (int i = 0; i < Guns.Count; i++)
        {
            var gun = Guns[i];
            gun.Initialize(this, Info.reloadTime);
        }

        StartCoroutine(FireGuns());
    }

    /// <summary>
    /// choose a new target - currently the nearest one possible
    /// </summary>
    public void ChooseNewTarget()
    {
        Enemy newTarget = null;
        float newTargetDistance = 0;

        // check through all the targets and get the closest one
        List<Enemy> targets = _enemyController._enemies;
        for (int i = 0; i < targets.Count; i++)
        {
            var target = targets[i];
            // make sure the target still exists
            if (!target)
            {
                Debug.Log("not there as a target");
                continue;
            }

            var targetDistance = Vector2.Distance(transform.position, target.Body.position);
            if (newTargetDistance == 0 || targetDistance < newTargetDistance)
            {
                // this is the closest target now
                newTarget = target;
                newTargetDistance = targetDistance;
            }
        }

        if (newTarget != null)
        {
            _currentEnemy = newTarget;
            _facingTarget = false;
            _firingState = TurretInfo.FiringState.aiming;
        }
    }

    // @TODO currently only handles one gun correctly
    public virtual IEnumerator FireGuns()
    {
        var gunCount = Guns.Count;
        var reloadTime = Info.reloadTime;
        int i = 0;
        while (true)
        {
            yield return new WaitUntil(() => _facingTarget);

            if (Guns[i].TryFire())
            {
                _firingState = TurretInfo.FiringState.firing;
                i = gunCount % (i + 1);

                yield return new WaitForSeconds(reloadTime);
            }
            else
            {
                continue;
            }

        }
    }

    public void HitEnemy()
    {
        ResetTurret();
        if (_currentEnemy)
        {
            _currentEnemy.TakeHit(Info.damage, 0);
        }
    }

    /*
    ---------------------------------------------
    ALL TEMP
    ---------------------------------------------
     */


    // code for turning the turret towards the target ----- handle by controller
    public void Turn()
    {
        if (!_currentEnemy)
        {
            ResetTurret();
            return;
        }

        Vector2 targetPos = _currentEnemy.Body.position;

        if (_firingState != TurretInfo.FiringState.firing)
        {
            // do not attempt to turn towards something out of the range
            var targetDistance = Vector2.Distance(transform.position, targetPos);
            var totalRange = Info.range + 0.5f; // add a bit of range to exclude the tile the turret is on
            if (targetDistance > totalRange)
            {
                ResetTurret();
                return;
            }
        }

        // find the desired angle to be facing the target @TODO factor in gun position on turret
        var desiredAngle = Angle.Towards(transform.position, targetPos);

        // leeway of 1 degree
        var currentDiff = Top.rotation.eulerAngles.z - desiredAngle.degrees;
        _facingTarget = -1 <= currentDiff && currentDiff <= 1;

        // rotate towards the target by the spin speed
        if (!_facingTarget)
        {
            Top.rotation = Quaternion.RotateTowards(Top.rotation, desiredAngle.AsQuaternion(), Info.spinSpeed * Time.deltaTime);
        }
    }

    private void ResetTurret()
    {
        _firingState = TurretInfo.FiringState.none;
        _facingTarget = false;
    }
}
