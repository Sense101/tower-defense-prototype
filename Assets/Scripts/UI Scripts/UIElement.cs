using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    // what to scale from
    public enum ScalingBase { canvas, parent }

    [Tooltip("the scaling type the element should use")]
    public ScalingBase scalingBase = ScalingBase.canvas;

    [Space(5)]
    [Tooltip("whether this element is primary and blocks inputs")]
    public bool primaryElement = false;


    [Space(5)]
    public ElementScale scale;
    public ElementPosition position;
    // public ElementFlex flex - coming soon :tm:

    // references
    [HideInInspector]
    public UIElement baseElement = null;
    public RectTransform RectTransform { get; private set; }

    public void Initialize()
    {
        // set the rect transform
        RectTransform = GetComponent<RectTransform>();
    }
}
