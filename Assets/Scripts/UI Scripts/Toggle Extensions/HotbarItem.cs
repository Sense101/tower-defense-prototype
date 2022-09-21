using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HotbarItemToggle))]
public class HotbarItem : ToggleExtension
{
    // set in inspector
    public GameObject turretPrefab;
    public Image icon;
    public TextMeshProUGUI costText;

    // internal variables
    ConfigController _configs;
    Turret _turret;

    private void Start()
    {
        _configs = ConfigController.Instance;
        _turret = turretPrefab.GetComponent<Turret>();

        (toggle as HotbarItemToggle).turret = _turret;

        icon.sprite = _turret.info.fullSprite;
        costText.text = "<sprite name='Coin'> " + _turret.info.cost.ToString();

        CoinController.Instance.onCoinsChanged.AddListener(UpdateItem);
        UpdateItem(CoinController.Instance.GetCoins());
    }

    public void UpdateItem(int amount)
    {
        // check if we can still afford it
        if (_configs.debug.noCoinCost || _turret.info.cost <= amount)
        {
            toggle.interactable = true;
            costText.color = _configs.color.canAffordColor;

            if (toggle.hovering)
            {
                toggle.OnHoverStart();
            }
        }
        else
        {
            toggle.interactable = false;
            costText.color = _configs.color.cannotAffordColor;

            // turn off
            toggle.isOn = false;
        }
    }
}
