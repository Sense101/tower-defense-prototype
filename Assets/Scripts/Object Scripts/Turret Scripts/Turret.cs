using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour
{
    const float TURN_LEEWAY = 0.5f;

    // set in inspector
    public TurretInfo Info;
    [SerializeField] Transform Top = default; // the part of the turret that rotates
    public List<Gun> Guns = new List<Gun>();


    private Enemy _currentTarget = null;
    private int _currentGunIndex = 0;
    private bool _facingTarget = false;
    public TurretInfo.FiringState _firingState = TurretInfo.FiringState.none;

    EnemyController _enemyController;


    // reload time / number of guns = wait time between shots - UNLESS OVERRIDEN

    /// <summary>
    /// initialize the turret
    /// </summary>
    public void Initialize(EnemyController enemyController)
    {
        // set the reference to the enemies
        _enemyController = enemyController;

        for (int i = 0; i < Guns.Count; i++)
        {
            var gun = Guns[i];
            gun.Initialize(this, Info.ReloadSpeed);
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

        // find all the targets in range
        List<Enemy> targets = _enemyController._enemies.FindAll(x => {
            return Vector2.Distance(transform.position, x.Body.position) <= Info.Range;
        });

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
            _currentTarget = newTarget;
            _facingTarget = false;
            _firingState = TurretInfo.FiringState.aiming;
        }
    }

    // @TODO currently only handles one gun correctly
    public virtual IEnumerator FireGuns()
    {
        var gunCount = Guns.Count;
        var reloadSpeed = Info.ReloadSpeed;
        int i = 0;
        while (true)
        {
            yield return new WaitUntil(() => _facingTarget);

            if (Guns[i].TryFire())
            {
                _firingState = TurretInfo.FiringState.firing;
                i = gunCount % (i + 1);

                yield return new WaitForSeconds(reloadSpeed);
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
        if (_currentTarget)
        {
            _currentTarget.TakeHit(Info.Damage, 0);
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
        if (!_currentTarget)
        {
            ResetTurret();
            return;
        }

        Vector2 targetPos = _currentTarget.Body.position;

        if (_firingState != TurretInfo.FiringState.firing)
        {
            // do not attempt to turn towards something out of the range
            var targetDistance = Vector2.Distance(transform.position, targetPos);
            var totalRange = Info.Range;
            if (targetDistance > totalRange)
            {
                ResetTurret();
                return;
            }
        }

        // find the desired angle to be facing the target @TODO factor in gun position on turret
        var desiredAngle = Angle.Towards(transform.position, targetPos);

        // slight leeway - we can't expect it be be exact
        var currentDiff = Top.rotation.eulerAngles.z - desiredAngle.degrees;
        _facingTarget = -TURN_LEEWAY <= currentDiff && currentDiff <= TURN_LEEWAY;

        // rotate towards the target by the spin speed
        if (!_facingTarget)
        {
            Top.rotation = Quaternion.RotateTowards(Top.rotation, desiredAngle.AsQuaternion(), Info.SpinSpeed * Time.deltaTime);
        }
    }

    private void ResetTurret()
    {
        _firingState = TurretInfo.FiringState.none;
        _facingTarget = false;
    }
}
