using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HotbarItemToggle : UIToggle
{
    // set by toggle extension
    public Turret turret;

    public override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretPlacer.Instance.SetTurretPrefab(turret);
            TurretPlacer.Instance.turretPreview.sprite = turret.info.fullSprite;
        }
        else
        {
            TurretPlacer.Instance.DeselectTurret();
            if (!hovering)
            {
                OnHoverEnd();
            }
        }
    }

    public override void OnSilentValueChanged(bool isOn)
    {
        OnHoverEnd();
    }

    public override void OnHoverStart()
    {
        transform.DOScale(new Vector2(1.2f, 1.2f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        if (!isOn)
        {
            transform.DOScale(new Vector2(1, 1), 0.1f);
        }
    }
}
