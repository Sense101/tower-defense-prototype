﻿using UnityEngine;

/// <summary>
/// Instance of each enemy
/// </summary>
public class Enemy : MonoBehaviour
{
    // set in inspector
    public EnemyInfo Info;
    public Transform Body;
    public HealthBar HealthBar;

    // the side of the path the enemy is on
    public EnemyInfo.PathSide pathSide;

    // location info
    public Angle directionAngle;
    public Vector2 nextTile;

    // health
    public int previewDamage = 0; // damage that hasn't taken effect yet
    public int currentHealth = 0; // actual health

    // TURN INFO
    public EnemyInfo.TurnProgress turnProgress = EnemyInfo.TurnProgress.none;
    public float turnPadding = 0;
    public int turnDirection = 0; // 1 or -1
    public Angle turnDirectionAngle; // the angle we are turning towards
    private float turnAmount = 0;

    /// <summary>
    /// initializes the enemy
    /// </summary>
    public void Initialize(EnemyInfo.PathSide pathSide, Angle directionAngle, Camera camera)
    {
        this.pathSide = pathSide;
        this.directionAngle = directionAngle;

        currentHealth = Info.Health;

        nextTile = (Vector2)transform.position + directionAngle.ToVector();

        HealthBar.Initialize(camera);
    }

    /// <summary>
    /// moves the enemy forwards each frame
    /// </summary>
    public void MoveForwards(Vector2 currentPos)
    {
        var moveSpeedDelta = Info.MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(currentPos, nextTile, moveSpeedDelta);
    }

    // puts everything in position to pivot around the parent
    public void StartTurn(float gapFromEdge)
    {
        var toPivot = (Vector3)(turnDirectionAngle.ToVector() * gapFromEdge);
        transform.position += toPivot;
        Body.position -= toPivot;
        HealthBar.transform.position -= toPivot;

        turnAmount = 0;
    }

    // called by update while turning
    public void Turn(float gapFromEdge)
    {
        // in tiles - πd
        var turningDistance = Mathf.PI * gapFromEdge;

        // how much we need to turn per second
        var degreesPerSecond = (Info.MoveSpeed * 90) / turningDistance;

        var degreesDelta = degreesPerSecond * Time.deltaTime;
        turnAmount += degreesDelta;

        // -turnDirection because angles are anti-clockwise
        transform.Rotate(new Vector3(0, 0, degreesDelta * -turnDirection));
        HealthBar.transform.Rotate(new Vector3(0, 0, degreesDelta * turnDirection), Space.Self);

        if (turnAmount >= 90f)
        {
            // this should result in the next frame no longer turning
            directionAngle.degrees = turnDirectionAngle.degrees;
            transform.rotation = turnDirectionAngle.AsQuaternion();
            transform.position = nextTile;
            Body.localPosition = Vector2.zero;
            HealthBar.transform.localPosition = Vector2.zero;
            HealthBar.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    /// <summary>
    /// reduces the enemies health, destroying it if its dead
    /// </summary>
    public void TakeHit(int damage, int armorPiercing)
    {
        var armor = Mathf.Max(0, Info.Armor - armorPiercing);
        var finalDamage = damage - armor;

        currentHealth -= finalDamage;

        HealthBar.SetFill((float)currentHealth / Info.Health);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}