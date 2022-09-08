using UnityEngine;

/// <summary>
/// info about a specific turret upgrade
/// </summary>
[CreateAssetMenu(menuName = "Infos/UpgradeInfo")]
public class UpgradeInfo : ScriptableObject
{
    // the id of the upgrade
    public string id;

    // the name of the augment
    public string title = "";
    public string shortDescription = "";
    [TextArea(2, 10)] public string longDescription = "";

    [Space(5)]

    public int cost = 0;

    // this augment's unique color - this will be applied to the turret when there are two or more of this on it
    // it will also color the background of the augment in the UI
    public Color color = Color.white;
}
