using UnityEngine;

/// <summary>
/// I know this is confusing, but this stores the info of augmentations on turrets DURING runtime
/// </summary>
[System.Serializable]
public class AugmentationInfo
{
    // the augmentation object
    public Augment augmentation;

    public int currentTier = 0;
}
