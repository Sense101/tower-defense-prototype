using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(HotbarItemToggle))]
public class HotbarItem : ToggleExtension
{
    // set in inspector
    public GameObject turretPrefab;
    public Image icon;
    public TextMeshProUGUI costText;

    private void Start()
    {
        Turret turret = turretPrefab.GetComponent<Turret>();
        (toggle as HotbarItemToggle).turret = turret;

        icon.sprite = turret.info.fullSprite;
        costText.text = turret.info.cost.ToString();
    }
}
