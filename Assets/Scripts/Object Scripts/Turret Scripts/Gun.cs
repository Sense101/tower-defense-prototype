using System.Collections;
using UnityEngine;

/// <summary>
/// script for each individual gun on a turret
/// </summary>
public class Gun : PoolObject
{
    public enum State { none, waitingToFire, firing, reloading }

    // set in inspector
    public SpriteRenderer body;
    public SpriteRenderer augmentSprite;

    [Space(5)]
    // internal variables
    public State state = State.none;
    public float remainingReloadTime = 0;
    public Bullet currentBullet = null; //@TODO store bullet reference

    [Space(5)]
    // stores stats which change each time the turret is updated
    public GunStatistics stats;

    public override void Activate()
    {
        state = State.waitingToFire;

        stats.currentTurret.guns.Add(this);
    }

    public override void Deactivate()
    {
        state = State.none;

        stats.currentTurret.guns.Remove(this);

        // hide sprites
        body.sprite = null;
        augmentSprite.sprite = null;
    }

    public override void Reset()
    {
        state = State.waitingToFire;

        body.transform.localPosition = stats.bodyDefaultPosition;
        remainingReloadTime = 0;
        StopAllCoroutines();
    }

    public void Fire(float reloadTime)
    {
        StartCoroutine(FireGun(reloadTime));
    }
    private IEnumerator FireGun(float reloadTime, float fireTime = 0.1f)
    {
        state = State.firing;

        remainingReloadTime = reloadTime;
        float actualReloadTime = reloadTime - fireTime;

        // fire!
        while (remainingReloadTime > actualReloadTime)
        {
            remainingReloadTime -= Time.deltaTime;

            float remainingFireTime = remainingReloadTime - actualReloadTime;
            float remainingDistanceMultiplier = Mathf.Clamp01(remainingFireTime / fireTime);

            body.transform.localPosition = Vector2.MoveTowards
            (
                stats.bodyFirePosition,
                stats.bodyDefaultPosition,
                stats.cachedDistance * remainingDistanceMultiplier
            );

            // wait a frame
            yield return null;
        }


        // firing is finished - start reloading straight away
        StartCoroutine(ReloadGun(actualReloadTime));
    }

    private IEnumerator ReloadGun(float actualReloadTime)
    {
        state = State.reloading;

        while (remainingReloadTime > 0)
        {
            remainingReloadTime -= Time.deltaTime;

            float remainingDistanceMultiplier = remainingReloadTime / actualReloadTime;

            body.transform.localPosition = Vector2.MoveTowards
            (
                stats.bodyDefaultPosition,
                stats.bodyFirePosition,
                stats.cachedDistance * remainingDistanceMultiplier
            );

            // wait a frame, if we're not done
            if (remainingReloadTime > 0)
            {
                yield return null;
            }
        }

        // ready to fire again
        remainingReloadTime = 0;
        state = State.waitingToFire;
    }
}
