using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIButton : Button
{
    public bool hovering = false;

    protected override void Start()
    {
        base.Start();

        if (Application.isPlaying)
        {
            SetReferences();
        }

        onClick.AddListener(OnClickInternal);
    }

    protected virtual void SetReferences() { }

    private void OnClickInternal()
    {
        OnClick();
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

    public abstract void OnHoverStart();
    public abstract void OnHoverEnd();
    public abstract void OnClick();
}
