using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// selects between UI button children
/// </summary>
[RequireComponent(typeof(UIElement))]
public class UIButtonSelector : MonoBehaviour
{
    private List<UIButton> _buttons = new List<UIButton>();
    public UIButton _selectedButton = null;

    //script to select one
    private void Start()
    {
        // find all the buttons and make sure navigation is horizontal
        for (int i = 0; i < transform.childCount; i++)
        {
            UIButton button = null;
            transform.GetChild(i).TryGetComponent<UIButton>(out button);
            if (button)
            {
                //Navigation newNavigation = new Navigation();
                //newNavigation.mode = Navigation.Mode.Horizontal;
                //newNavigation.wrapAround = true;
                //button.navigation = newNavigation;
                _buttons.Add(button);
            }
        }

        // add onClick to buttons
        for (int i = 0; i < _buttons.Count; i++)
        {
            UIButton button = _buttons[i];
            int index = i;
            button.onClick.AddListener(() => OnClick(index));
        }
    }

    public void OnClick(int index)
    {
        SelectButton(index);
    }

    public void SelectButton(int index)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            UIButton button = _buttons[i];
            if (i == index)
            {
                if (!button.Selected)
                {
                    _selectedButton = button;
                    button.Selected = true;
                    button.OnSelect();
                }
            }
            else if (button.Selected)
            {
                button.Selected = false;
                button.OnDeselect();
            }
        }
    }


    public void DeselectAll()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            UIButton button = _buttons[i];
            if (button.Selected)
            {
                button.Selected = false;
                button.OnDeselect();
            }
        }
    }
}
