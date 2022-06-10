using UnityEngine;

public class AugmentationButton : UIButton
{
    public Augmentation augmentation;
    public int index = 0;

    TurretInterface _turretInterface;

    public override void SetReferences()
    {
        _turretInterface = TurretInterface.Instance;
    }

    public override void OnClick()
    {
        _turretInterface.ApplyAugmentation(augmentation, index);
    }
}
