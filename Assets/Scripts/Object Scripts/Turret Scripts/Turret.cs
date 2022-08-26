using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Turret : PoolObject
{
    // enums
    public enum State { none, lookingForTarget, aiming, locked, firing }
    public enum TargetType { close, strong, weak, random }

    [HideInInspector] public UnityEvent onBulletHitEvent = new UnityEvent();

    // set in inspector
    public TurretInfo info;
    public Transform top; // the part of the turret that rotates
    public List<Gun> guns = new List<Gun>();

    // renderers - set in inspector
    public SpriteRenderer turretBase;
    public SpriteRenderer body;
    public SpriteRenderer gunMount;


    [Space(10)]
    // info unique to this turret
    public Enemy currentTarget = null;
    public int currentGunIndex = 0;
    public State state = Turret.State.none;
    public TargetType targetType = TargetType.close;
    private Coroutine _firingRoutine = null;

    [Space(10)]
    // info about the augments
    public int damageAugmentLevel = 0;
    public int rangeAugmentLevel = 0;
    public int customAugmentLevel = 0;
    public UpgradeInfo[] upgrades = new UpgradeInfo[3];

    [Space(10)]
    // and the actual statistics - this is what augments will modify
    public TurretStatistics stats;

    public override void SetReferences()
    {
        // inizialize statistics
        stats = new TurretStatistics();

        //_bulletController = BulletController.Instance;
        //_enemyController = EnemyController.Instance;
    }


    public override void Activate()
    {
        // start looking for a target
        state = State.lookingForTarget;

        // start firing guns
        StartFiringRoutine();

        // show everything
        turretBase.color = Color.white;
        body.color = new HSVColor(147, 77, 80).AsColor(); //@TODO should this be on the controller?
        gunMount.color = Color.white;
    }

    public override void Deactivate()
    {
        // prevent it doing anything
        state = State.none;

        // stop firing guns
        StopFiringRoutine();

        // hide completely
        turretBase.color = Color.clear;
        body.color = Color.clear;
        gunMount.color = Color.clear;


        //@TODO what about clearing preview damage?
    }

    public override void Reset()
    {
        targetType = TargetType.close;
        stats.Reset();
        currentTarget = null;
        currentGunIndex = 0;
    }

    public override void SetRotation(Angle rotation)
    {
        // the rotation of the *top*
        top.rotation = rotation.AsQuaternion();
    }

    public void StartFiringRoutine()
    {
        // start the firing coroutine
        if (_firingRoutine == null)
        {
            _firingRoutine = StartCoroutine(FireGuns());
        }
    }
    public void StopFiringRoutine()
    {
        // stop the firing routine
        if (_firingRoutine != null)
        {
            StopCoroutine(_firingRoutine);
            _firingRoutine = null;
        }
    }
    private IEnumerator FireGuns()
    {
        while (true)
        {
            // wait till we've found a target
            if (state == State.lookingForTarget)
            {
                yield return new WaitUntil(() => state != State.lookingForTarget);
            }

            int gunCount = guns.Count;
            float reloadSpeed = stats.reloadTime;

            // wait till we are locked onto a target and we can fire
            if (!CanFire(guns[currentGunIndex]))
            {
                yield return new WaitUntil(() => CanFire(guns[currentGunIndex]));
            }

            // check if the target will die already
            if (currentTarget.IsSpaceForDamage())
            {
                Gun currentGun = guns[currentGunIndex];

                // fire!
                state = State.firing;
                currentGun.Fire(stats.reloadTime);

                // spawn bullet
                BulletStatistics newBulletStats = new BulletStatistics(currentTarget, stats.damage, stats.armorPiercing);
                Vector2 spawnPos = currentGun.transform.position;
                Angle spawnAngle = new Angle(currentGun.transform.rotation);
                Bullet newBullet = BulletController.Instance.CreateBullet(info.bulletInfo, newBulletStats, spawnPos, spawnAngle, currentGun.transform);
                newBullet.onHitEvent.AddListener(damage => OnBulletHit(newBullet, damage));

                // wait till firing is complete
                yield return new WaitUntil(() => currentGun.state == Gun.State.reloading);

                // don't do damage to the enemy - the bullet will handle it

                // prevent firing of next gun till we hit a nicer ratio - temp, allow this to be modified in future
                yield return new WaitUntil(() => currentGun.remainingReloadTime <= reloadSpeed / 2);
            }
            else
            {
                // this target will already die, find another target
                state = State.lookingForTarget;
            }
        }
    }

    private bool CanFire(Gun gun)
    {
        return state == State.locked && gun.state == Gun.State.waitingToFire;
    }

    // called when a bullet hit something
    public void OnBulletHit(Bullet b, int damage)
    {
        stats.xp += damage;
        onBulletHitEvent.Invoke();

        // now destroy the bullet, its done its job
        b.onHitEvent.RemoveAllListeners();
        BulletController.Instance.DestroyBullet(b);

        // now that we've hit, start looking for a new target
        state = State.lookingForTarget;
    }
}

/*
NOTE TO SELF: code is a mess, tidy up before doing anything else

- add gun stats with all stuff it needs to know
- replace gun animations with animancer
- clean up firing coroutine, now that we are oing things differently
- maybe cache gun count on turret?
- try and find a way we can avoid restarting coroutine every time we upgrade
- just generally make upgrading work nicer, with gun list etc
- and comment all the ode afterwards too

- it's worth getting this done cleanly!
leaves 12:50 terminal 1
arrives 8:05 terminal 3
EZY1827

*/
