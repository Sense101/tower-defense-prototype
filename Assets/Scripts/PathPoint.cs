using UnityEngine;

/// <summary>
/// a point in the path the enemies walk
/// </summary>
public class PathPoint : MonoBehaviour
{
    // set in inspector
    public PathPoint nextPoint;
    public bool endPoint = false;

    public Vector2 cachedWorldPos;

    public void Start()
    {
        cachedWorldPos = transform.position;
    }

    public void OnEnemyArrive(Enemy enemy)
    {
        if (endPoint)
        {
            //@TODO don't destroy
            Destroy(enemy.gameObject);
        }
        else
        {
            enemy.targetPoint = nextPoint;
            Angle towardsPoint = Angle.Towards(enemy.transform.position, nextPoint.transform.position);
            enemy.SetRotation(towardsPoint);
        }
    }
}
