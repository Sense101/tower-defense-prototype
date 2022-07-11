using System.Collections.Generic;
using UnityEngine;

public class GunController : ObjectPoolHandlerSingleton<GunController, Gun>
{
    // "creates" a gun, for a turret to use
    public Gun CreateGun(Transform parent, GunInfo info, Turret parentTurret)
    {
        // create
        Gun newGun = CreateObject(Vector2.zero, Angle.zero, parent);

        // modify to our specifications
        ModifyGun(newGun, info, parentTurret);

        // activate
        newGun.Activate();

        return newGun;
    }

    // modifies a gun
    public void ModifyGun(Gun g, GunInfo info, Turret parentTurret)
    {
        // match info
        g.body.sprite = info.sprite;
        g.transform.localPosition = info.localPosition;
        g.transform.localRotation = new Angle(info.localRotation).AsQuaternion();

        // modify stats
        g.stats.Modify(parentTurret, info.bodyDefaultPosition, info.bodyFirePosition);

        // reset
        g.Reset();
    }

    // "deletes" a gun
    public void DeactivateGun(Gun g)
    {
        g.SetParent(transform);
        DeactivateObject(g);
    }
}
