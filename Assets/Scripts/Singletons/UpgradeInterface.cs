using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInterface : UIOverlaySingleton<UpgradeInterface>
{
    // variables
    public int currentSlotIndex = 0;

    private UpgradeSelection[] _upgradeSelections;

    // references
    CanvasFadeGroup _fadeGroup;
    UpgradeController _upgradeController;
    TurretInterface _turretInterface;
    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _upgradeController = UpgradeController.Instance;
        _turretInterface = TurretInterface.Instance;

        // set upgrade selections and apply onclick
        _upgradeSelections = GetComponentsInChildren<UpgradeSelection>();
        Debug.Log(_upgradeSelections.Length);
        for (int i = 0; i < _upgradeSelections.Length; i++)
        {
            UpgradeSelection selection = _upgradeSelections[i];
            Debug.Log(i);

            // seperate out to a new variable for the lambda to capture
            int index = i;
            selection.button.onClick.AddListener(() => ChooseUpgrade(index));
        }
    }

    protected override void OpenInternal()
    {
        UpgradeInfo[] upgrades = _upgradeController.upgrades;

        // update previews - still work @todo here
        for (int i = 0; i < _upgradeSelections.Length; i++)
        {
            UpgradeSelection selection = _upgradeSelections[i];

            if (upgrades.Length > i)
            {
                UpgradeInfo upgradeInfo = upgrades[i];

                // enable the selection
                selection.gameObject.SetActive(true);

                // apply info so it matches
                selection.backgroundImage.color = upgradeInfo.color;
                //selection.icon = @todo
                selection.titleText.text = upgradeInfo.title;
            }
            else
            {
                // disable, we don't need it
                selection.gameObject.SetActive(false);
            }
        }

        _fadeGroup.Show();
    }
    protected override void CloseInternal()
    {
        _fadeGroup.Hide();
    }

    // called by buttons when we choose an upgrade
    public void ChooseUpgrade(int index)
    {
        Turret currentTurret = _turretInterface.GetTurret();

        if (currentTurret.upgrades.Length <= index)
        {
            Debug.LogError("Trying to apply an upgrade to a turret slot that does not exist!");
            return;
        }

        UpgradeInfo chosenUpgrade = _upgradeController.upgrades[index];

        currentTurret.upgrades[currentSlotIndex] = chosenUpgrade;
        _upgradeController.ApplyUpgrade(currentTurret.stats, chosenUpgrade.id);
        _turretInterface.UpdateInterface();
        // @todo now we need to visualise the change in the turret interface

        // done, now we close
        Close();
    }
}
