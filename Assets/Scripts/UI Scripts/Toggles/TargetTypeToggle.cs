using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetTypeToggle : UIToggle
{
    // set by toggle extension
    public Turret.TargetType targetType;

    Image backgroundImage;

    public override void SetReferences()
    {
        backgroundImage = GetComponent<Image>();
    }

    public override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretInterface.Instance.SetTurretTargetType(targetType);
            backgroundImage.DOColor(ConfigController.Instance.color.targetTypeSelectedColor, 0.1f);
        }
        else
        {
            SetToDefault();
        }
    }

    public override void OnSilentValueChanged(bool isOn)
    {
        SetToDefault();
    }

    private void SetToDefault()
    {
        backgroundImage.DOColor(ConfigController.Instance.color.targetTypeDefaultColor, 0.1f);
        if (!hovering)
        {
            OnHoverEnd();
        }
    }

    public override void OnHoverStart()
    {
        if (!isOn)
        {
            transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
        }
    }
    public override void OnHoverEnd()
    {
        transform.DOScale(Vector2.one, 0.1f);
    }
}
