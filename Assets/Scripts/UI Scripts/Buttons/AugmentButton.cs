using UnityEngine;

public class AugmentButton : UIButton
{

    TurretInterface _turretInterface;
    AugmentInterface _augmentInterface;

    protected override void SetReferences()
    {
        _turretInterface = TurretInterface.Instance;
        _augmentInterface = AugmentInterface.Instance;
    }

    protected override void OnClick()
    {
        _turretInterface.ChooseAugment();
        //_augmentInterface.Show();
    }
}
