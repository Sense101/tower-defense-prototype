using UnityEngine;

public class HotbarButton : UIButton
{
    public Turret turretPrefab = null;
    public Sprite turretPreviewSprite = null;
    private TurretPlacer _turretPlacer;

    protected override void SetReferences()
    {
        _turretPlacer = TurretPlacer.Instance;
    }

    protected override void OnSelect()
    {
        base.OnSelect();

        // set the current turret to ours
        _turretPlacer.SetTurretPrefab(turretPrefab);
        _turretPlacer.turretPreview.sprite = turretPreviewSprite;
    }
}
