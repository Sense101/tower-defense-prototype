using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private List<UIElement> elements = new List<UIElement>();
    private List<UIButton> buttons = new List<UIButton>();
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
        foreach (UIElement element in Elements)
        {
            // initialize
        }
        foreach (UIButton button in Buttons)
        {
            button.Initialize();
        }
    }

    public bool IsWithinPrimaryElement(Vector2 pos)
    {
        List<UIElement> primaryElements = Elements.FindAll(x => x.primaryElement);
        foreach (UIElement element in primaryElements)
        {
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

    private void Update()
    {

    }
}
