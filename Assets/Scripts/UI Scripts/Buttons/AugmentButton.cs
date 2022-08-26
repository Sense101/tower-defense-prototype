using UnityEngine;

public class AugmentButton : UIButton
{
    public override void OnClick()
    {
        TurretInterface.Instance.ChooseAugment();
        //_augmentInterface.Show();
    }

    public override void OnHoverStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnHoverEnd()
    {
        throw new System.NotImplementedException();
    }
}
