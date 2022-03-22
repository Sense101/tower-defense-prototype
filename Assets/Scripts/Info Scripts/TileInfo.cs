[System.Serializable]
public class TileInfo
{
    public bool path { get; private set; }
    public bool placeable { get; private set; }
    public Turret turret = null;

    public TileInfo(bool path, bool placeable)
    {
        this.path = path;
        this.placeable = placeable;
    }
}
