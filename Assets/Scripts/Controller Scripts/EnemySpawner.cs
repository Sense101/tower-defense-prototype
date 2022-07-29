using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    // set in inspector
    public List<WaveInfo> waves = new List<WaveInfo>();
    public PathPoint firstPoint;

    EnemyController _enemyController;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = EnemyController.Instance;

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("spawning now!");

        for (int w = 0; w < waves.Count; w++)
        {
            var wave = waves[w];

            for (int g = 0; g < wave.SpawnGroups.Count; g++)
            {
                var group = wave.SpawnGroups[g];

                for (int i = 0; i < group.SpawnAmount; i++)
                {
                    Vector2 spawnPos = transform.position;
                    Angle spawnAngle = new Angle(transform.rotation);

                    Enemy newEnemy = _enemyController.CreateEnemy(
                        spawnPos,
                        spawnAngle,
                        group.EnemyInfo,
                        firstPoint
                    );

                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}
