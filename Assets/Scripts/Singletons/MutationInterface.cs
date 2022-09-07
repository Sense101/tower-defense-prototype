using UnityEngine;
using UnityEngine.UI;

public class MutationInterface : UIOverlaySingleton<MutationInterface>
{
    // set in inspector
    [SerializeField] Image[] pathImages;

    // references
    CanvasFadeGroup _fadeGroup;
    TurretController _turretController;
    TurretInterface _turretInterface;
    TurretPlacer _turretPlacer;
    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _turretController = TurretController.Instance;
        _turretInterface = TurretInterface.Instance;
        _turretPlacer = TurretPlacer.Instance;
    }

    protected override void OpenInternal()
    {
        Turret currentTurret = _turretInterface.GetTurret();

        // update previews - still work @todo here
        for (int i = 0; i < pathImages.Length; i++)
        {
            Image pathImage = pathImages[i];

            if (currentTurret.info.mutationPaths.Length > i)
            {
                TurretInfo mutationInfo = currentTurret.info.mutationPaths[i];

                // enable the path image @todo this is not nice
                pathImage.transform.parent.gameObject.SetActive(true);
                pathImage.sprite = mutationInfo.fullSprite;
            }
            else
            {
                pathImage.transform.parent.gameObject.SetActive(false);
            }
        }

        _fadeGroup.Show();
    }
    protected override void CloseInternal()
    {
        _fadeGroup.Hide();
    }

    // called by upgrade path buttons @todo make this handled by us!
    public void SelectPath(int pathIndex)
    {
        Turret currentTurret = _turretInterface.GetTurret();

        if (!currentTurret)
        {
            return;
        }

        // upgrade the turret
        TurretInfo newInfo = currentTurret.info.mutationPaths[pathIndex];
        _turretController.ModifyTurret(currentTurret, newInfo);
        _turretInterface.UpdateInterface();

        // deselect, our job is done
        Close();
        _turretPlacer.TryDeselectTurret();
        MainInterface.Instance.hotbar.DeselectAll();
    }
}
