using UnityEngine;

public class TargetTypeButton : UIButton
{
    public Turret.TargetType targetType = Turret.TargetType.close;
    private TurretInterface _turretInterface;

    public override void SetReferences()
    {
        _turretInterface = TurretInterface.Instance;
    }

    public override void OnSelect()
    {
        // set the target type of the selected turret
        Turret selected = _turretInterface.selectedTurret;
        if (selected)
        {
            selected.targetType = targetType;
        }
    }
}
