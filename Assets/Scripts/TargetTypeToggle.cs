using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTypeToggle : GroupToggle
{
    //set in inspector
    [SerializeField] Turret.TargetType _targetType;

    protected override void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            TurretInterface.Instance.SetTurretTargetType(_targetType);
        }

        // notify parent
        base.OnValueChanged(isOn);
    }
}
