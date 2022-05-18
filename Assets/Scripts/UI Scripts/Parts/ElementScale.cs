using UnityEngine;

/// <summary>
/// element part that holds UI scaling variables
/// </summary>
[System.Serializable]
public class ElementScale : ElementPart
{
    // our sizing
    [Tooltip("the desired size if the ratio is 16:9")]
    public Vector2 defaultSize = new Vector2(1, 1);

    [Space(5)]

    [Tooltip("the width increase per each extra tile of width")]
    public float widthIncrease = 0;

    [Tooltip("the height per each extra tile of height")]
    public float heightIncrease = 0;
}
