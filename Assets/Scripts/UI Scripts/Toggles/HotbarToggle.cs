using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HotbarToggle : UIToggle
{
    // set in inspector
    [SerializeField] Turret _turretPrefab = null;
    [SerializeField] Sprite _turretPreviewSprite = null;

    public override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretPlacer.Instance.SetTurretPrefab(_turretPrefab);
            TurretPlacer.Instance.turretPreview.sprite = _turretPreviewSprite;
        }
        else
        {
            TurretPlacer.Instance.TryDeselectTurret();
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
