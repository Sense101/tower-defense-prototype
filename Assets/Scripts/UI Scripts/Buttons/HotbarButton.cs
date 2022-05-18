using UnityEngine;

public class HotbarButton : UIButton
{
    public GameObject TurretPrefab = null;
    public Sprite TurretPreviewSprite = null;
    private TurretPlacer _turretPlacer;

    public override void SetReferences()
    {
        _turretPlacer = TurretPlacer.Instance;
    }

    public override void OnSelect()
    {
        // set the current turret to ours
        _turretPlacer.CurrentTurretPrefab = TurretPrefab;
        _turretPlacer.turretPreview.sprite = TurretPreviewSprite;
    }
}
