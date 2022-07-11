using UnityEngine;

public class TargetTypeButton : UIButton
{
    public Turret.TargetType targetType = Turret.TargetType.close;
    private TurretInterface _turretInterface;

    protected override void SetReferences()
    {
        _turretInterface = TurretInterface.Instance;
    }

    protected override void OnSelect()
    {
        base.OnSelect();

        // set the target type of the selected turret
        Turret selected = _turretInterface.GetTurret();
        if (selected)
        {
            selected.targetType = targetType;
        }
    }
}
