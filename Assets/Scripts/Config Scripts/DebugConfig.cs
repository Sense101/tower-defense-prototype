using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/DebugConfig")]
public class DebugConfig : ScriptableObject
{
    // eventually add in any config I might need

    public bool noCoinCost = false;
    public bool instantMutations = false;
    [Min(0)] public float playSpeedMultiplier = 1;
}
