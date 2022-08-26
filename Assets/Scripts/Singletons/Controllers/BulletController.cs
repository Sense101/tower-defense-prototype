using UnityEngine;

public class BulletController : ObjectPoolHandlerSingleton<BulletController, Bullet>
{
    // creates a bullet with the bullet controller as the parent
    public Bullet CreateBullet(BulletInfo info, BulletStatistics newStatistics, Vector2 location, Angle rotation)
    {
        return CreateBullet(info, newStatistics, location, rotation, transform);
    }

    // creates a bullet and activates it
    public Bullet CreateBullet(BulletInfo info, BulletStatistics newStatistics, Vector2 location, Angle rotation, Transform parent)
    {
        Bullet newBullet = CreateObject(location, rotation, parent);

        ModifyBullet(newBullet, info, newStatistics);

        newBullet.Activate();

        return newBullet;
    }

    public void ModifyBullet(Bullet b, BulletInfo info, BulletStatistics newStatistics)
    {
        // match info
        b.body.sprite = info.sprite;
        b.SetController(info.controller);

        b.stats = newStatistics;
    }

    public void DestroyBullet(Bullet b)
    {
        DeactivateObject(b);
    }

    // will do a lot of the heavy lifting for bullet handling
}
