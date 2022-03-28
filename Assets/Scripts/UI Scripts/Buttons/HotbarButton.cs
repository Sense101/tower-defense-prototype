using UnityEngine;

public class HotbarButton : UIButton
{
    public GameObject TurretPrefab = null;

    public override void OnSelect()
    {
        // set the current turret to ours
        TurretPlacer.Instance.CurrentTurretPrefab = TurretPrefab;
    }
}
