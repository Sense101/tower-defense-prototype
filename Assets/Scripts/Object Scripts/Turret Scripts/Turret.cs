using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Turret : PoolObject
{
    // enums
    public enum State { none, aiming, locked, firing }
    public enum TargetType { close, first, strong, weak }

    [HideInInspector] public UnityEvent onHitEvent = new UnityEvent();

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
    public State state = State.none;
    public TargetType targetType = TargetType.close;

    [Space(10)]
    public AugmentationInfo augment;

    [Space(10)]
    // and the actual statistics - this is what augments will modify
    public TurretStatistics statistics;

    public override void SetReferences()
    {
        // set gun references
        foreach (Gun gun in guns)
        {
            gun.SetReferences(this);
        }
    }


    public override void Activate()
    {
        // inizialize statistics
        statistics = new TurretStatistics(info);

        // initialize guns
        foreach (Gun gun in guns)
        {
            gun.Activate();
            gun.SetReloadSpeed(statistics.reloadSpeed);
        }

        StartCoroutine(FireGuns());

        // show everything
        turretBase.color = Color.white;
        body.color = new HSVColor(147, 77, 80).AsColor(); //@TODO should this be on the controller?
        gunMount.color = Color.white;
    }

    public override void Deactivate()
    {
        // should be removed from turret controller before this is called

        // stop firing guns
        StopAllCoroutines();

        // deactivate guns
        foreach (Gun gun in guns)
        {
            gun.Deactivate();
        }

        // hide completely
        turretBase.color = Color.clear;
        body.color = Color.clear;
        gunMount.color = Color.clear;


        //@TODO what about clearing preview damage?
    }

    public override void Reset()
    {
        currentTarget = null;
        currentGunIndex = 0;
        state = State.none;
        targetType = TargetType.close;
        statistics.Reset(info);
    }

    public override void SetRotation(Angle rotation)
    {
        top.rotation = rotation.AsQuaternion();
    }

    public virtual IEnumerator FireGuns()
    {
        var gunCount = guns.Count;
        var reloadSpeed = statistics.reloadSpeed;
        while (true)
        {
            // wait till we are locked onto a target
            yield return new WaitUntil(() => state == State.locked);

            // then try and fire
            if (currentTarget.IsSpaceForDamage() && guns[currentGunIndex].TryFire())
            {
                state = State.firing;

                // add preview damage
                currentTarget.previewDamage += statistics.damage; //@TODO what if preview damage changes while firing, maybe wait till not firing before applying change

                // wait till we are done firing the current gun
                yield return new WaitUntil(() => state != State.firing);

                // prevent firing of next gun till we hit a nicer ratio
                // reload time / number of guns = wait time between shots
                yield return new WaitForSeconds(reloadSpeed / gunCount);
            }

        }
    }

    // called by the gun
    public void HitEnemy()
    {
        // reset the turret
        state = State.none;

        // make sure the enemy exists
        if (currentTarget)
        {
            // remove preview and hit the enemy
            currentTarget.previewDamage -= statistics.damage;
            currentTarget.TakeHit(statistics.damage, 0);

            // add xp
            statistics.xp += statistics.damage;

            onHitEvent.Invoke();
        }
    }
}
