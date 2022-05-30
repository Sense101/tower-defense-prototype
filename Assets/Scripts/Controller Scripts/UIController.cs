using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public const string CANVAS_TAG = "MainCanvas";

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
        UIElement canvasElement = GameObject.FindWithTag(CANVAS_TAG).GetComponent<UIElement>();
        foreach (UIElement e in Elements)
        {
            // set the base transform
            if (e.scalingBase == UIElement.ScalingBase.canvas)
            {
                e.baseElement = canvasElement;
            }
            else
            {
                UIElement parentElement = e.transform.parent.GetComponent<UIElement>();
                if (!parentElement)
                {
                    Debug.LogError($"The parent of element '{gameObject.name}' is not a UI element!");
                }
                e.baseElement = parentElement;
            }
            e.Initialize();
        }
        foreach (UIButton b in Buttons)
        {
            b.Initialize();
        }
    }

    public bool IsWithinPrimaryElement(Vector2 pos)
    {
        List<UIElement> primaryElements = Elements.FindAll(x => x.primaryElement);
        foreach (UIElement element in primaryElements)
        {
            CanvasFadeGroup fadeGroup = element.GetComponent<CanvasFadeGroup>();
            if (fadeGroup && fadeGroup.state == CanvasFadeGroup.FadeState.hidden)
            {
                // skip if it's not visible
                continue;
            }

            // the distance from the center of the element to the edge, including scaling
            Vector2 elementBounds = element.RectTransform.sizeDelta * element.RectTransform.lossyScale / 2; //@TODO cache this so we only need to check it once, and also filter out inactive ones
            float xDistance = Mathf.Abs(element.RectTransform.position.x - pos.x);
            float yDistance = Mathf.Abs(element.RectTransform.position.y - pos.y);

            // if the distance to the element is less than its bounds, we are within it
            bool withinBounds = xDistance <= elementBounds.x && yDistance <= elementBounds.y;
            if (withinBounds)
            {
                return true;
            }
        }
        return false;
    }
}
