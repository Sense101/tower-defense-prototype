using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UpgradeSlotButton))]
public class UpgradeSlot : ButtonExtension
{
    // set in inspector
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI titleText;

    private void Start()
    {
    }

    public void SetSlotIndex(int index)
    {
        (button as UpgradeSlotButton).slotIndex = index;
    }
}
