using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleToggle : UIToggle
{
    public override void OnValueChanged(bool isOn)
    {
        // do nothing
    }

    public override void OnHoverStart()
    {
        transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        transform.DOScale(new Vector2(1, 1), 0.1f);
    }
}
