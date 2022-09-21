using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInterface : UIOverlaySingleton<UpgradeInterface>
{
    // variables
    public int currentSlotIndex = 0;

    // internal variables
    UpgradeSelection[] _upgradeSelections;

    // references
    CanvasFadeGroup _fadeGroup;
    UpgradeController _upgradeController;
    TurretInterface _turretInterface;
    CoinController _coinController;
    ConfigController _configs;

    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _upgradeController = UpgradeController.Instance;
        _turretInterface = TurretInterface.Instance;
        _coinController = CoinController.Instance;
        _configs = ConfigController.Instance;

        // set upgrade selections and apply onclick
        _upgradeSelections = GetComponentsInChildren<UpgradeSelection>();
        for (int i = 0; i < _upgradeSelections.Length; i++)
        {
            UpgradeSelection selection = _upgradeSelections[i];

            // seperate out to a new variable for the lambda to capture
            int index = i;
            selection.button.onClick.AddListener(() => ChooseUpgrade(index));
        }
    }

    protected override void OpenInternal()
    {
        UpgradeInfo[] upgrades = _upgradeController.upgrades;

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
                selection.descText.text = upgradeInfo.shortDescription;
                selection.descText.color = upgradeInfo.color;
                selection.costText.text = upgradeInfo.cost.ToString();

                // check if we can afford the upgrade
                if (_configs.debug.noCoinCost || _coinController.CanAfford(upgradeInfo.cost))
                {
                    selection.costText.color = _configs.color.canAffordColor;
                    selection.button.interactable = true;
                }
                else
                {
                    selection.costText.color = _configs.color.cannotAffordColor;
                    selection.button.interactable = false;
                }
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

        // deduct coins
        if (!_configs.debug.noCoinCost)
        {
            if (!_coinController.TrySpendCoins(chosenUpgrade.cost))
            {
                Debug.LogError("Tried to apply upgrade without enough coins, this should not be possible!");
                return;
            }
        }

        // add to the sell amount
        currentTurret.stats.sellAmount += chosenUpgrade.cost / 2;

        // add the upgrade to the turret and apply
        currentTurret.upgrades[currentSlotIndex] = chosenUpgrade;
        _upgradeController.ApplyUpgrade(currentTurret.stats, chosenUpgrade.id);

        // update the interface to visualise changes
        _turretInterface.UpdateInterface();


        // done, now we close
        Close();
    }
}
