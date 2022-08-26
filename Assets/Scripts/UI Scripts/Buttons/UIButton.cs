using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIButton : Button
{
    public bool hovering = false;

    protected override void Start()
    {
        base.Start();

        onClick.AddListener(OnClickInternal);
    }

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
