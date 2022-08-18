using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupToggleSelector : MonoBehaviour
{
    // set in inspector
    public bool allowSwitchOff = false;

    public GroupToggle[] childToggles;

    private void Start()
    {
        childToggles = GetComponentsInChildren<GroupToggle>();

        foreach (GroupToggle toggle in childToggles)
        {
            toggle.parentToggleSelector = this;
        }
    }

    public void OnToggleChange(GroupToggle toggle, bool isOn)
    {
        if (isOn)
        {
            // make sure all other items are off
            foreach (GroupToggle otherToggle in childToggles)
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

    public GroupToggle SelectByIndex(int index)
    {
        GroupToggle toggleByIndex = childToggles[index];
        if (!toggleByIndex)
        {
            return null;
        }

        toggleByIndex.SetIsOn(true);
        return toggleByIndex;
    }
}
