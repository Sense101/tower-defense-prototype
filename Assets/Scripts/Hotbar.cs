using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//@todo this is all temp
public class Hotbar : UIToggleSelector
{
    public void DeselectAll()
    {
        foreach (UIToggle toggle in childToggles)
        {

            toggle.SetIsOn(false, true);
        }
    }
}
