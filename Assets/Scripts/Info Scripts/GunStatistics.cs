using UnityEngine;

[System.Serializable]
public class GunStatistics
{
    // the turret we are currently attached to
    public Turret currentTurret = null;

    // the positions the gun moves to
    public Vector2 bodyDefaultPosition = Vector2.zero;
    public Vector2 bodyFirePosition = Vector2.zero;

    // the cached distance between the default and fire position
    public float cachedDistance = 0;

    // modifies the stored info to match something new
    public void Modify(Turret newTurret, Vector2 newDefaultPosition, Vector2 newFirePosition)
    {
        currentTurret = newTurret;
        bodyDefaultPosition = newDefaultPosition;
        bodyFirePosition = newFirePosition;
        cachedDistance = Vector2.Distance(newDefaultPosition, newFirePosition);
    }
}
