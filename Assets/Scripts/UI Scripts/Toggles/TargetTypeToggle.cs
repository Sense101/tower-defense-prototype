using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetTypeToggle : UIToggle
{
    //set in inspector
    [SerializeField] Turret.TargetType _targetType;

    public override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretInterface.Instance.SetTurretTargetType(_targetType);
            OnHoverStart();
        }
        else if (!hovering)
        {
            OnHoverEnd();
        }
    }

    public override void OnSilentValueChanged(bool isOn)
    {
        OnHoverEnd();
    }

    public override void OnHoverStart()
    {
        transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
    }
    public override void OnHoverEnd()
    {
        if (!isOn)
        {
            transform.DOScale(Vector2.one, 0.1f);
        }
    }
}
