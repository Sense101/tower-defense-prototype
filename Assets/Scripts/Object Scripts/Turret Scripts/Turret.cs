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
    public TargetType targetType = TargetType.close;

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
            // wait till we are locked onto a target
            yield return new WaitUntil(() => state == State.locked);

            if (currentTarget.IsSpaceForDamage() && Guns[currentGunIndex].TryFire())
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
    }
}
