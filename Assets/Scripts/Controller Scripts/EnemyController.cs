using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves all enemies and tells them where to turn next, does not handle damage
/// </summary>
public class EnemyController : ObjectPoolHandlerSingleton<EnemyController, Enemy>
{
    public readonly List<Enemy> activeEnemies = new List<Enemy>();


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
        foreach (Enemy e in activeEnemies)
        {
            MoveEnemy(e);
        }
    }

    public Enemy CreateEnemy(Vector2 location, Angle rotation, EnemyInfo info, PathPoint nextPoint)
    {
        Enemy newEnemy = CreateObject(location, rotation, transform);
        ModifyEnemy(newEnemy, info);

        newEnemy.targetPoint = nextPoint;
        newEnemy.Activate();

        activeEnemies.Add(newEnemy);

        return newEnemy;
    }

    protected override Enemy InstantiateObject(Vector2 location, Angle rotation, Transform parent)
    {
        // instantiate
        Enemy newObject = Instantiate(objectPrefab, location, rotation.AsQuaternion(), parent);

        // set references - custom for enemies
        newObject.SetReferences(_mainCamera);

        // add to list
        objects.Add(newObject);

        return newObject;
    }

    public void ModifyEnemy(Enemy e, EnemyInfo newInfo)
    {
        e.info = newInfo;
        e.body.sprite = newInfo.sprite;
    }

    public void DestroyEnemy(Enemy e)
    {
        activeEnemies.Remove(e);
        DeactivateObject(e);
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
