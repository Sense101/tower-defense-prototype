using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[DisallowMultipleComponent]
public class GroupToggle : MonoBehaviour
{
    // reference back to parent toggle selector
    [HideInInspector] public GroupToggleSelector parentToggleSelector;

    protected Toggle _toggle;
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void SetIsOn(bool selected, bool silent = false)
    {
        if (silent)
        {
            _toggle.SetIsOnWithoutNotify(selected);
        }
        else
        {
            _toggle.isOn = selected;
        }
    }

    protected virtual void OnValueChanged(bool isOn)
    {
        // notify parent hotbar
        parentToggleSelector.OnToggleChange(this, isOn);
    }
}
