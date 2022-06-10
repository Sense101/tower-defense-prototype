using UnityEngine;

/// <summary>
/// class for augmentations to derive from
/// </summary>
[System.Serializable]
public class AugmentationModifier
{
    /// <summary>
    /// is this actually an active modifier
    /// </summary>
    public bool active = false;

    /// <summary>
    /// whether this modifier is a multiplier or a simple addition
    /// </summary>
    public bool multiplier = false;

    /// <summary>
    /// the actual modifier amount
    /// </summary>
    public float amount = 0;
}
