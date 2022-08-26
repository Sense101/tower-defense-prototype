using UnityEngine;

/// <summary>
/// info about a specific turret upgrade
/// </summary>
[CreateAssetMenu]
public class UpgradeInfo : ScriptableObject
{
    public string id;
    public enum Type
    {
        damage, range
    }

    // the type of the augment, this controls what it will actually do to the turret
    public Type type = Type.damage;

    // the name of the augment
    public string title = "";

    // this augment's unique color - this will mix with others on a turret
    public Color color = Color.white; //@TODO should also have option of not applying color at all

    // stores descriptions of what the tier upgrade actually does
    public string[] tierDescriptions = new string[5];
}
