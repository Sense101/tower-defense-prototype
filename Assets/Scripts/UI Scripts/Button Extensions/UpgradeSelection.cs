using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// this is placed on the selections for choosing an upgrade
public class UpgradeSelection : ButtonExtension
{
    // set in inspector
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI costText;
}
