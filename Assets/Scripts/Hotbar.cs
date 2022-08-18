using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//@todo this is all temp
public class Hotbar : GroupToggleSelector
{
    public void DeselectAll()
    {
        foreach (GroupToggle toggle in childToggles)
        {

            toggle.SetIsOn(false, true);
        }
    }
}
