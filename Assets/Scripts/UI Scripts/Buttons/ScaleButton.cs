using DG.Tweening;
using UnityEngine;

public class ScaleButton : UIButton
{
    public override void OnClick()
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
