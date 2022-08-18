using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : Singleton<UIController>
{

    private List<UIElement> elements = new List<UIElement>();
    private List<UIButton> buttons = new List<UIButton>();
    //@TODO remove this montrosity
    public List<UIElement> Elements
    {
        get
        {
            if (elements.Count < 1)
            {
                elements = GetComponentsInChildren<UIElement>().ToList();
            }
            return elements;
        }
    }
    public List<UIButton> Buttons
    {
        get
        {
            if (buttons.Count < 1)
            {
                buttons = GetComponentsInChildren<UIButton>().ToList();
            }
            return buttons;
        }
    }

    // references set in inspector
    [Header("References")]
    public UIButtonSelector hotbar = default;

    private void Start()
    {
        //UIElement canvasElement = GameObject.FindWithTag(CANVAS_TAG).GetComponent<UIElement>();
        //foreach (UIElement e in Elements)
        //{
        //    // set the base transform
        //    if (e.scalingBase == UIElement.ScalingBase.canvas)
        //    {
        //        e.baseElement = canvasElement;
        //    }
        //    else
        //    {
        //        UIElement parentElement = e.transform.parent.GetComponent<UIElement>();
        //        if (!parentElement)
        //        {
        //            Debug.LogError($"The parent of element '{gameObject.name}' is not a UI element!");
        //        }
        //        e.baseElement = parentElement;
        //    }
        //    e.Initialize();
        //}
        //foreach (UIButton b in Buttons)
        //{
        //    b.Initialize();
        //}
    }
}
