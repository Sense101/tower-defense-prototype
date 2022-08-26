using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves all enemies and tells them where to turn next, does not handle damage
/// </summary>
public class EnemyController : Singleton<EnemyController>
{
    public List<Enemy> enemies = new List<Enemy>();


    Map _map;
    TurretController _turretController;
    Camera _mainCamera;

    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var e = enemies[i];
            if (!e)
            {
                enemies.RemoveAt(i);
                continue;
            }

            MoveEnemy(e);
        }
    }

    public Enemy SpawnEnemy(Vector2 location, Angle rotation, GameObject prefab, PathPoint nextPoint)
    {
        Enemy newEnemy = Instantiate(prefab, location, Quaternion.identity, transform).GetComponent<Enemy>();
        newEnemy.targetPoint = nextPoint;

        newEnemy.SetBodyRotation(rotation);
        newEnemy.SetReferences(_mainCamera);
        newEnemy.EnemyStart();

        Add(newEnemy);

        return newEnemy;
    }

    /// <summary>
    /// adds an enemy to the array
    /// </summary>
    public void Add(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private void MoveEnemy(Enemy e)
    {
        float moveSpeedDelta = e.info.MoveSpeed * Time.deltaTime;
        Vector2 target = e.targetPoint.cachedWorldPos;

        e.transform.position = Vector2.MoveTowards(e.transform.position, target, moveSpeedDelta);

        // now check to see if we need to change path point
        if ((Vector2)e.transform.position == target)
        {
            e.targetPoint.OnEnemyArrive(e);
        }
    }
}
