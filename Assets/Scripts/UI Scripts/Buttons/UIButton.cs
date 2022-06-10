using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button
{
    const string ANIMATOR_SELECTED = "selected";
    const string ANIMATOR_HIGHLIGHTED = "highlighted";
    private bool _selected;
    public bool Selected
    {
        get
        {
            // check to see if we are out of sync with animator
            // @TODO do we need this? or should we never pull from animator
            if (_animator)
            {
                _selected = animator.GetBool(ANIMATOR_SELECTED);
            }
            return _selected;
        }
        set
        {
            _selected = value;
            _animator?.SetBool(ANIMATOR_SELECTED, value);
        }
    }

    Animator _animator;

    public virtual void Initialize()
    {
        TryGetComponent<Animator>(out _animator);
        onClick.AddListener(OnClick);
        SetReferences();
    }

    public virtual void SetReferences() { }

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnHoverStart();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        OnHoverEnd();
    }
    public virtual void OnHoverStart()
    {
        _animator?.SetBool(ANIMATOR_HIGHLIGHTED, true);
    }
    public virtual void OnHoverEnd()
    {
        _animator?.SetBool(ANIMATOR_HIGHLIGHTED, false);
    }

    public virtual void OnClick() { }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Debug.Log("pointer down");
    }
}
