using UnityEngine;
using UnityEngine.Events;

public class Bullet : PoolObject
{
    const string ACTIVATE_TRIGGER = "activate";
    public enum Type { fake, real }

    // set in inspector
    public SpriteRenderer body;

    // internal variables
    public Type type = Type.fake;
    public BulletStatistics stats;

    // called when we hit something, passing in the damage
    [HideInInspector] public UnityEvent<int> onHitEvent = new UnityEvent<int>();

    Animator _animator;

    public override void SetReferences()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        // start firing
        stats.target.previewDamage += stats.damage;
        _animator.SetTrigger(ACTIVATE_TRIGGER);
    }

    public override void Deactivate()
    {
        _animator.StopPlayback();
        body.sprite = null;
    }

    public void SetController(RuntimeAnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }

    // called by the animation
    public void OnAnimationComplete()
    {
        // actually damage the enemy
        Enemy e = stats.target;
        if (e)
        {
            e.previewDamage -= stats.damage;
            e.TakeHit(stats.damage, stats.armorPiercing);

            onHitEvent.Invoke(stats.damage);
        }
        else
        {
            onHitEvent.Invoke(0);
        }

    }
}
