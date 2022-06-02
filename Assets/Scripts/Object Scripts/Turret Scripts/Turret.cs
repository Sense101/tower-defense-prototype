using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Turret : MonoBehaviour
{
    // enums
    public enum State { none, aiming, locked, firing }
    public enum TargetType { close, first, strong, weak }

    [HideInInspector] public UnityEvent onHitEvent = new UnityEvent();

    // set in inspector
    public TurretInfo info;
    public Transform top; // the part of the turret that rotates
    public SpriteRenderer[] renderers; //@TODO what use is this?
    public List<Gun> guns = new List<Gun>();


    [Space(10)]
    // info unique to this turret
    public Enemy currentTarget = null;
    public int currentGunIndex = 0;
    public State state = State.none;
    public TargetType targetType = TargetType.close;

    [Space(5)]
    // and the actual statistics - this is what augments will modify
    public TurretStatistics statistics;


    /// <summary>
    /// initialize the turret
    /// </summary>
    public void Initialize()
    {
        // inizialize statistics
        statistics = new TurretStatistics(info.Damage, info.SpinSpeed, info.ReloadSpeed);

        // inizialize guns
        for (int i = 0; i < guns.Count; i++)
        {
            var gun = guns[i];
            gun.Initialize(this, statistics.reloadSpeed);
        }

        StartCoroutine(FireGuns());
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
