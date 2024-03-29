using UnityEngine;

public class MutationInterface : UIOverlaySingleton<MutationInterface>
{
    // internal variables
    MutationSelection[] _mutationSelections;

    // references
    CanvasFadeGroup _fadeGroup;
    TurretInterface _turretInterface;
    CoinController _coinController;
    ConfigController _configs;
    private void Start()
    {
        // set references
        _fadeGroup = GetComponent<CanvasFadeGroup>();
        _turretInterface = TurretInterface.Instance;
        _coinController = CoinController.Instance;
        _configs = ConfigController.Instance;

        // set mutation selections and apply onclick
        _mutationSelections = GetComponentsInChildren<MutationSelection>();
        for (int i = 0; i < _mutationSelections.Length; i++)
        {
            MutationSelection selection = _mutationSelections[i];

            // seperate out to a new variable for the lambda to capture
            int index = i;
            selection.button.onClick.AddListener(() => ChoosePath(index));
        }
    }

    protected override void OpenInternal()
    {
        Turret currentTurret = _turretInterface.GetTurret();

        // update previews - still work @todo here
        for (int i = 0; i < _mutationSelections.Length; i++)
        {
            MutationSelection selection = _mutationSelections[i];

            if (currentTurret.info.mutationPaths.Length > i)
            {
                selection.gameObject.SetActive(true);

                TurretInfo mutationInfo = currentTurret.info.mutationPaths[i];

                // apply info so it matches
                selection.icon.sprite = mutationInfo.fullSprite;
                selection.titleText.text = mutationInfo.title;
                selection.costText.text = mutationInfo.cost.ToString();

                // check if we can afford the mutation
                if (_configs.debug.noCoinCost || _coinController.CanAfford(mutationInfo.cost))
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

    public void ChoosePath(int pathIndex)
    {
        Turret currentTurret = _turretInterface.GetTurret();

        if (!currentTurret)
        {
            return;
        }

        TurretInfo newInfo = currentTurret.info.mutationPaths[pathIndex];

        // deduct the coins for the mutation
        if (!_configs.debug.noCoinCost && !_coinController.TrySpendCoins(newInfo.cost))
        {
            Debug.LogError("Can not afford mutation- it should not be clickable!");
        }

        // upgrade the turret
        TurretController.Instance.ModifyTurret(currentTurret, newInfo);

        // add to the sell amount
        currentTurret.stats.sellAmount += newInfo.cost / 2;

        // reset the turret xp
        currentTurret.stats.xp = 0;

        // update the interface to match our changes
        _turretInterface.UpdateInterface();

        Close();
    }
}
