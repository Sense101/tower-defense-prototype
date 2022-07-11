using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// class for buttons, override methods differently depending on what a button does
public class UIButton : Button
{
    private bool _selected = false;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            if (_selected == value)
            {
                return;
            }
            _selected = value;

            // call methods when state changes
            if (value)
            {
                OnSelect();
            }
            else
            {
                OnDeselect();
            }
        }
    }

    private bool _mouseHovering = false;
    public bool Hovering
    {
        get => _mouseHovering;
    }

    public virtual void Initialize()
    {
        onClick.AddListener(OnClick);
        SetReferences();
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _mouseHovering = true;

        if (interactable)
        {
            OnHoverStart();
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        _mouseHovering = false;
        OnHoverEnd();
    }

    // methods to be overriden
    protected virtual void SetReferences() { }
    protected virtual void OnSelect()
    {
        // default implementation - decrease the size
        transform.localScale = new Vector2(0.9f, 0.9f);
    }
    protected virtual void OnDeselect()
    {
        // default implementation - go back to standard state, whether hobvering or not
        if (Hovering)
        {
            OnHoverStart();
        }
        else
        {
            OnHoverEnd();
        }
    }
    protected virtual void OnHoverStart()
    {
        // default implementation, increase the size of the button when hovering
        if (!Selected)
        {
            transform.localScale = new Vector2(1.1f, 1.1f);
        }
    }
    protected virtual void OnHoverEnd()
    {
        // default implementation, reset the size of the button after hover
        if (!Selected)
        {
            transform.localScale = Vector2.one;
        }
    }
    protected virtual void OnClick() { }
}
