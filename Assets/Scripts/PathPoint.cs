using UnityEngine;

/// <summary>
/// a point in the path the enemies walk
/// </summary>
public class PathPoint : MonoBehaviour
{
    // set in inspector
    public PathPoint nextPoint;
    public float maximumOffset = 5;

    public Angle cachedAngleToNext;
    public float cachedDistanceToNext = 0;
    public float cachedDistanceToEnd = 0;

    protected virtual void Awake()
    {
        cachedAngleToNext = Angle.Towards(transform.position, nextPoint.transform.position);
        cachedDistanceToNext = Vector2.Distance(transform.position, nextPoint.transform.position);
    }

    protected virtual void Start()
    {
        PathPoint currentPoint = this;
        while (currentPoint.nextPoint)
        {
            cachedDistanceToEnd += currentPoint.cachedDistanceToNext;
            currentPoint = currentPoint.nextPoint;
        }
    }

    public virtual void OnEnemyArrive(Enemy enemy)
    {
        enemy.targetPoint = nextPoint;
        enemy.targetPosition = GetNextPosition(enemy.offset);
        enemy.SetBodyRotation(Angle.Towards(enemy.transform.position, enemy.targetPosition));
    }

    public Vector2 GetNextPosition(float offset)
    {
        // shorten offset to the maximum size for the next point
        if (Mathf.Abs(offset) > nextPoint.maximumOffset)
        {
            offset = nextPoint.maximumOffset * Mathf.Sign(offset);
        }

        // angle to the next one, rotated to become horizontal offset, then as a vector 
        Vector2 finalOffset = cachedAngleToNext.CloneRotate(-90).AsVector() * offset;

        // if there is a point after the next, factor that in as well
        if (nextPoint.nextPoint)
        {
            finalOffset += nextPoint.cachedAngleToNext.CloneRotate(-90).AsVector() * offset;
        }

        Vector2 nextPosition = (Vector2)nextPoint.transform.position + finalOffset;
        return nextPosition;
    }
}
