using UnityEngine;
using UnityEngine.UI;

public class MutationInterface : Singleton<MutationInterface>
{
    // set in inspector
    [SerializeField] Image[] pathImages;

    // internal variables
    private Turret _selectedTurret;

    // references
    CanvasFadeGroup _fadeGroup;
    TurretController _turretController;
    TurretPlacer _turretPlacer;
    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _turretController = TurretController.Instance;
        _turretPlacer = TurretPlacer.Instance;
    }

    public void Show(Turret turret)
    {
        _selectedTurret = turret;

        // update previews - still work @todo here
        for (int i = 0; i < pathImages.Length; i++)
        {
            Image pathImage = pathImages[i];

            if (_selectedTurret.info.mutationPaths.Length > i)
            {
                TurretInfo mutationInfo = _selectedTurret.info.mutationPaths[i];

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
    public void Hide()
    {
        _selectedTurret = null;

        _fadeGroup.Hide();
    }

    // called by upgrade path buttons
    public void SelectPath(int pathIndex)
    {
        if (!_selectedTurret)
        {
            return;
        }

        // upgrade the turret
        TurretInfo newInfo = _selectedTurret.info.mutationPaths[pathIndex];
        _turretController.ModifyTurret(_selectedTurret, newInfo);

        // deselect, our job is done
        _turretPlacer.TryDeselectTurret();
        MainInterface.Instance.hotbar.DeselectAll();
    }
}
