using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves all enemies and tells them where to turn next, does not handle damage
/// </summary>
public class EnemyController : Singleton<EnemyController>
{
    public readonly List<Enemy> _enemies = new List<Enemy>();
    Map _map;
    TurretController _turretController;
    float gapFromEdge;

    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        gapFromEdge = EnemySpawner.gapFromEdge;
    }

    private void Update()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            if (!enemy)
            {
                _enemies.RemoveAt(i);
                continue;
            }

            UpdateEnemy(enemy, enemy.transform.position);
        }
    }

    private void UpdateEnemy(Enemy enemy, Vector2 currentPos)
    {
        if (enemy.turnProgress == EnemyInfo.TurnProgress.none)
        {
            if (currentPos == enemy._nextTile && !CheckForCorner(enemy, currentPos))
            {
                SetNextTile(enemy, currentPos);
            }
            enemy.MoveForwards(currentPos);
        }
        else
        {
            if (currentPos == enemy._nextTile)
            {
                if (enemy.turnProgress == EnemyInfo.TurnProgress.starting)
                {
                    enemy.turnProgress = EnemyInfo.TurnProgress.turning;

                    var endOffset = (enemy._directionAngle.ToVector() + enemy.turnDirectionAngle.ToVector()) * gapFromEdge;
                    enemy._nextTile = currentPos + endOffset;

                    enemy.StartTurn(gapFromEdge);

                }
                else if (enemy.turnProgress == EnemyInfo.TurnProgress.turning)
                {
                    enemy.turnProgress = EnemyInfo.TurnProgress.ending;
                    SetNextTile(enemy, currentPos, enemy.turnPadding);
                }
                else if (enemy.turnProgress == EnemyInfo.TurnProgress.ending)
                {
                    enemy.turnProgress = EnemyInfo.TurnProgress.none;
                    SetNextTile(enemy, currentPos);
                }
            }

            if (enemy.turnProgress == EnemyInfo.TurnProgress.turning)
            {
                enemy.Turn(gapFromEdge);
            }
            else
            {
                enemy.MoveForwards(currentPos);
            }
        }
    }

    /// <summary>
    /// adds an enemy to the array
    /// </summary>
    public void Add(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    /// <summary>
    /// checks in front of the enemy for a corner
    /// </summary>
    /// <returns>whether a corner was found</returns>
    private bool CheckForCorner(Enemy enemy, Vector2 currentPos)
    {
        /*  --- first check for a path up and to the side --- */

        // find the vector to the tile we are checking
        var turnDirection = new Vector2Int();
        switch (enemy._pathSide)
        {
            case EnemyInfo.PathSide.left:
                turnDirection = new Vector2Int(-1, 0);
                break;
            case EnemyInfo.PathSide.right:
                turnDirection = new Vector2Int(1, 0);
                break;
            default:
                break;
        }

        // the angle relative to the enemy
        var turnAngle = new Angle(turnDirection).Rotate(enemy._directionAngle.degrees);

        // if the tile is a path - we are on a corner
        if (CheckTile(currentPos, turnAngle.ToVector(), true))
        {
            // set the distances for the turn
            enemy.turnPadding = 0;

            enemy.turnDirectionAngle = turnAngle;
            enemy.turnDirection = turnDirection.x;

            enemy.turnProgress = EnemyInfo.TurnProgress.starting;

            return true;
        }

        /*  --- now check for ground two ahead --- */

        // if the tile two ahead is ground - we are approaching a bend
        if (CheckTile(currentPos, enemy._directionAngle.ToVector() * 2, false))
        {
            // set the distances for the turn
            enemy.turnPadding = 1 - (gapFromEdge * 2);

            // turn direction and angle is opposite to above
            enemy.turnDirectionAngle = turnAngle.Rotate(180);
            enemy.turnDirection = -turnDirection.x;

            enemy.turnProgress = EnemyInfo.TurnProgress.starting;

            SetNextTile(enemy, currentPos, enemy.turnPadding);

            return true;
        }

        return false;
    }

    /// <summary>
    /// returns true if the tile is the same as specified
    /// </summary>
    private bool CheckTile(Vector2 currentPos, Vector2 tileOffset, bool isPath)
    {
        var tilePos = Vector2Int.RoundToInt(currentPos + tileOffset);
        var tile = _map.TryGetTileWorldSpace(tilePos);

        if (tile != null && tile.path == isPath) return true;

        return false;
    }

    /// <summary>
    /// set the next tile for the enemy to move to
    /// </summary>
    private void SetNextTile(Enemy enemy, Vector2 currentPos, float distanceMultiplier = 1)
    {
        enemy._nextTile = currentPos + (enemy._directionAngle.ToVector() * distanceMultiplier);
    }


    ///////////////////////////////
    // OBSOLETE
    ///////////////////////////////

    /// <summary>
    ///  update surrounding turrets so that they know to fire upon the enemy
    /// </summary>
    //private void UpdateTurrets(Enemy enemy, Vector2 currentPos)
    //{
    //    var turrets = turretController.turrets;
    //    for (int i = 0; i < turrets.Count; i++)
    //    {
    //        var turret = turrets[i];
    //
    //        // add one to the range because we are only checking every tile
    //        float detectionRange = turret.info.range + 1;
    //
    //        // find whether we are close enough to the turret
    //        float xDiff = currentPos.x - turret.transform.position.x;
    //        float yDiff = currentPos.y - turret.transform.position.y;
    //        bool withinX = -detectionRange <= xDiff && xDiff <= detectionRange;
    //        bool withinY = -detectionRange <= yDiff && yDiff <= detectionRange;
    //
    //        if (withinX && withinY)
    //        {
    //            // add this enemy to the list of targets for the turret
    //            if (!turret.targets.Contains(enemy)) turret.targets.Add(enemy);
    //        }
    //        else
    //        {
    //            turret.targets.Remove(enemy);
    //        }
    //    }
    //}
}
