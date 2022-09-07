using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarToggle : UIToggle
{
    // set in inspector
    [SerializeField] Turret _turretPrefab = null;
    [SerializeField] Sprite _turretPreviewSprite = null;

    protected override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretPlacer.Instance.SetTurretPrefab(_turretPrefab);
            TurretPlacer.Instance.turretPreview.sprite = _turretPreviewSprite;
        }
        else
        {
            TurretPlacer.Instance.TryDeselectTurret();
        }
    }

    protected override void OnHoverStart()
    {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }
    protected override void OnHoverEnd()
    {
        transform.localScale = Vector2.one;
    }

}
