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
        SetReferences();
    }

    public virtual void SetReferences() { }

    public virtual void OnSelect()
    {
        //Debug.Log("selected button");
    }
    public virtual void OnDeselect()
    {
        //Debug.Log("deselected button");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("highlight " + gameObject.name);
        _animator?.SetBool(ANIMATOR_HIGHLIGHTED, true);
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("stop highlight " + gameObject.name);
        _animator?.SetBool(ANIMATOR_HIGHLIGHTED, false);
        base.OnPointerExit(eventData);
    }
}
