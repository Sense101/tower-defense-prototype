using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public abstract class UIToggle : Toggle
{
    // reference back to parent toggle selector
    [HideInInspector] public UIToggleSelector parentToggleSelector;
    public bool hovering = false;

    protected override void Start()
    {
        base.Start();

        SetReferences();

        if (Application.isPlaying)
        {
            onValueChanged.AddListener(OnValueChangedInternal);
        }
    }

    public virtual void SetReferences() { }

    public void SetIsOn(bool isOn, bool silent = false)
    {
        if (silent)
        {
            SetIsOnWithoutNotify(isOn);
            OnSilentValueChanged(isOn);
        }
        else
        {
            this.isOn = isOn;
        }
    }

    private void OnValueChangedInternal(bool isOn)
    {
        OnValueChanged(isOn);

        // notify parent hotbar
        parentToggleSelector?.OnToggleChange(this, isOn);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        hovering = true;
        if (interactable)
        {
            OnHoverStart();
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        hovering = false;
        OnHoverEnd();
    }

    public abstract void OnValueChanged(bool isOn);
    public virtual void OnSilentValueChanged(bool isOn) { }

    public abstract void OnHoverStart();
    public abstract void OnHoverEnd();
}
