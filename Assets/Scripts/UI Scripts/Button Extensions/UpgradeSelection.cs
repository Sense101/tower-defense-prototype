using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//@TODO MAKE THIS BE A BUTTON SCRIPT!

// this is placed on the selections for choosing an upgrade
// this script will be kept as simple as possible, to allow the interface to do the real work
public class UpgradeSelection : ButtonExtension
{
    // set in inspector
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI titleText;

    /*
    so what do I need for upgrade selection?
    we will be changing: the color of the background sprite, the icon, the title text, the description text...?
    */
}
