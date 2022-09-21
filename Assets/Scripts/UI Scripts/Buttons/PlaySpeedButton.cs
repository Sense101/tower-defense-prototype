using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlaySpeedButton : UIButton
{
    private Transform _innerTransform; // @todo this issue needs a proper resolution
    private GameObject playIcon1;

    bool doubleSpeed = false;

    protected override void SetReferences()
    {
        _innerTransform = transform.GetChild(0);
        playIcon1 = _innerTransform.GetChild(0).gameObject;
        playIcon1.SetActive(false);
    }

    public override void OnClick()
    {
        doubleSpeed = !doubleSpeed;
        playIcon1.SetActive(doubleSpeed);

        TimeController.Instance.SetPlaySpeed(doubleSpeed ? 2 : 1);
    }

    public override void OnHoverStart()
    {
        _innerTransform.DOScale(new Vector2(0.9f, 0.9f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        _innerTransform.DOScale(new Vector2(1, 1), 0.1f);
    }
}
