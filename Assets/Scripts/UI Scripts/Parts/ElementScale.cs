using UnityEngine;

/// <summary>
/// element part that holds UI scaling variables
/// </summary>
[System.Serializable]
public class ElementScale
{
    // base transform
    [Tooltip("the rect transform that the scale is based off (defaults to the main canvas)")]
    public RectTransform BaseTransform = null;
    [HideInInspector] public UIElement CachedBaseElement = null;

    // our sizing
    [Space(5)]
    [Tooltip("the desired size if the ratio is 16:9")]
    public Vector2 DefaultSize = new Vector2(1, 1);

    [Space(5)]

    [Tooltip("the width increase per each extra tile of width")]
    public float WidthIncrease = 0;

    [Tooltip("the height per each extra tile of height")]
    public float HeightIncrease = 0;
}
