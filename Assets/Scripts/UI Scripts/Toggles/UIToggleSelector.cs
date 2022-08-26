using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleSelector : MonoBehaviour
{
    // set in inspector
    public bool allowSwitchOff = false;

    public UIToggle[] childToggles;

    private void Start()
    {
        childToggles = GetComponentsInChildren<UIToggle>();

        foreach (UIToggle toggle in childToggles)
        {
            toggle.parentToggleSelector = this;
        }
    }

    public void OnToggleChange(UIToggle toggle, bool isOn)
    {
        if (isOn)
        {
            // make sure all other items are off
            foreach (UIToggle otherToggle in childToggles)
            {
                if (toggle == otherToggle)
                {
                    continue;
                }

                otherToggle.SetIsOn(false, true);
            }
        }
        else if (!allowSwitchOff)
        {
            toggle.SetIsOn(true, true);
        }
    }

    public UIToggle SelectByIndex(int index)
    {
        UIToggle toggleByIndex = childToggles[index];
        if (!toggleByIndex)
        {
            return null;
        }

        toggleByIndex.SetIsOn(true);
        return toggleByIndex;
    }
}
