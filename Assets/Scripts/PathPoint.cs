using UnityEngine;

/// <summary>
/// a point in the path the enemies walk
/// </summary>
public class PathPoint : MonoBehaviour
{
    // set in inspector
    public PathPoint nextPoint;
    public float maximumOffset = 5;
    public bool endPoint = false;

    public void Start()
    {
        if (nextPoint)
        {
            transform.rotation = Angle.Towards(transform.position, nextPoint.transform.position).AsQuaternion();
        }
    }

    public void OnEnemyArrive(Enemy enemy)
    {
        if (endPoint)
        {
            //@TODO fully implement this
            Destroy(enemy.gameObject);
        }
        else
        {
            enemy.targetPoint = nextPoint;
            enemy.targetPosition = GetNextPosition(enemy.offset);
            Angle towardsPoint = Angle.Towards(enemy.transform.position, nextPoint.transform.position);
            enemy.SetBodyRotation(towardsPoint);
        }
    }

    public Vector2 GetNextPosition(float offset)
    {
        if (Mathf.Abs(offset) > nextPoint.maximumOffset)
        {
            offset = nextPoint.maximumOffset * Mathf.Sign(offset);
        }

        // find the rotation towards the next path point
        Angle toNextPoint = Angle.Towards(transform.position, nextPoint.transform.position);

        Vector2 finalOffset = toNextPoint.Rotate(-90).AsVector() * offset;

        // if there is a point after the next, factor that in as well
        if (nextPoint.nextPoint)
        {
            Angle afterNextPoint = Angle.Towards(nextPoint.transform.position, nextPoint.nextPoint.transform.position);
            finalOffset += afterNextPoint.Rotate(-90).AsVector() * offset;
        }

        Vector2 nextPosition = (Vector2)nextPoint.transform.position + finalOffset;
        return nextPosition;
    }
}
