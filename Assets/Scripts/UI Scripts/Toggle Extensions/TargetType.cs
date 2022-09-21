using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetTypeToggle))]
public class TargetType : ToggleExtension
{
    public Turret.TargetType targetType;

    private void Start()
    {
        (toggle as TargetTypeToggle).targetType = targetType;
    }
}
