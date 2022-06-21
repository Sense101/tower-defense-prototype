using System.Collections.Generic;
using UnityEngine;

public class GunController : ObjectPoolHandlerSingleton<GunController, Gun>
{
    // "creates" a gun, for a turret to use
    public Gun CreateGun(Transform parent, GunInfo info, float reloadSpeed)
    {
        // create
        Gun newGun = CreateObject(Vector2.zero, Angle.zero, parent);

        // update
        UpdateGun(newGun, info, reloadSpeed);

        // activate
        newGun.Activate(); /////////////// IMPLEMENT RELOAD SPEED AND GET GUNS INITIOALIZED WITHOUT TURRET

        return newGun;
    }

    // updates an active gun
    public void UpdateGun(Gun g, GunInfo info, float reloadSpeed)
    {
        g.transform.localPosition = info.localPosition;
        g.transform.localRotation = new Angle(info.localRotation).AsQuaternion();
        g.SetAnimatorController(info.animatorController);
        g.SetReloadSpeed(reloadSpeed);
    }

    // "deletes" a gun
    public void DeleteGun(Gun g)
    {
        g.SetParent(transform);
        DeactivateObject(g);
    }
}
