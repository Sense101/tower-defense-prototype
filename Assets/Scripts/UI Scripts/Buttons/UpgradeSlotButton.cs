using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeSlotButton : UIButton
{
    // is this slot currently selected - to choose an upgrade for it
    public bool selected;

    // set by the interface
    public int slotIndex = -1;

    // references
    UpgradeInterface _upgradeInterface;

    protected override void SetReferences()
    {
        _upgradeInterface = UpgradeInterface.Instance;
    }

    public override void OnClick()
    {
        if (slotIndex == -1)
        {
            Debug.LogError("Slot index not set!");
            return;
        }

        _upgradeInterface.currentSlotIndex = slotIndex;
        _upgradeInterface.Open();

        selected = true;
        _upgradeInterface.onClose.AddListener(OnUpgradeInterfaceClose);
    }

    private void OnUpgradeInterfaceClose()
    {
        _upgradeInterface.onClose.RemoveListener(OnUpgradeInterfaceClose);

        selected = false;
        if (!hovering)
        {
            OnHoverEnd();
        }
    }

    public override void OnHoverStart()
    {
        transform.DOScale(new Vector2(1.1f, 1.1f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        if (!selected)
        {
            transform.DOScale(new Vector2(1, 1), 0.1f);
        }
    }
}
