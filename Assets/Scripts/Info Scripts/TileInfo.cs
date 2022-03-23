[System.Serializable]
public class TileInfo
{
    public bool Path { get; private set; }
    public bool Placeable { get; private set; }
    private Turret _turret = null;
    public Turret Turret
    {
        get => _turret;
        set => _turret = value;
    }

    public TileInfo(bool path, bool placeable)
    {
        Path = path;
        Placeable = placeable;
    }
}
