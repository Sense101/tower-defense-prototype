using UnityEngine;

/// <summary>
/// info about a specific turret upgrade
/// </summary>
[CreateAssetMenu]
public class UpgradeInfo : ScriptableObject
{
    // the id of the upgrade
    public string id;

    // the name of the augment
    public string title = "";
    [TextArea(3, 10)] public string description = "";

    [Space(10)]

    // this augment's unique color - this will be applied to the turret when there are two or more of this on it
    // it will also color the background of the augment in the UI
    public Color color = Color.white;
}
