using UnityEngine;

/// <summary>
/// element part that holds position scaling variables
/// </summary>
[CreateAssetMenu]
public class ElementPosition : ElementPart
{
    [Tooltip("the default position relative to the anchor point")]
    public Vector2 DefaultPosition = new Vector2(0, 0);

    [Space(5)]

    [Tooltip("the x axis movement per each extra tile of width")]
    public float XMovement = 0;

    [Tooltip("the y axis movement per each extra tile of height")]
    public float YMovement = 0;
}
