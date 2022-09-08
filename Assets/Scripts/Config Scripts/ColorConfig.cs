using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ColorConfig")]
public class ColorConfig : ScriptableObject
{
    [Header("Placement")]
    public Color unblockedPlacementColor;
    public Color blockedPlacementColor;
    public Color unblockedRangeColor;
    public Color blockedRangeColor;

    [Header("Coins")]
    public Color canAffordColor;
    public Color cannotAffordColor;
}
