using UnityEngine;

public class AugmentationButton : UIButton
{

    TurretInterface _turretInterface;

    public override void SetReferences()
    {
        _turretInterface = TurretInterface.Instance;
    }

    public override void OnClick()
    {
        _turretInterface.ApplyAugmentation();
    }
}
