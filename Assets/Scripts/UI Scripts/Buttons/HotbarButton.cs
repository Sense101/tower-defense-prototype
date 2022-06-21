using UnityEngine;

public class HotbarButton : UIButton
{
    //@TOOO change to turret info eventually.. maybe???
    public Turret turretPrefab = null;
    public Sprite turretPreviewSprite = null;
    private TurretPlacer _turretPlacer;

    public override void SetReferences()
    {
        _turretPlacer = TurretPlacer.Instance;
    }

    public override void OnSelect()
    {
        // set the current turret to ours
        _turretPlacer.SetTurretPrefab(turretPrefab);
        _turretPlacer.turretPreview.sprite = turretPreviewSprite;
    }
}
