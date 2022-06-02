using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    float side = 1;
    public static float gapFromEdge
    {
        get { return 0.2f; }
    }

    public List<WaveInfo> waves = new List<WaveInfo>();

    EnemyController enemyController;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        side = Mathf.Sign(Random.Range(-1, 1));
        // we ALSO need a way to spawn enemies on both sides at once


        // for now I will just do a basic spawner that is only one and can't do multiple sides

        enemyController = EnemyController.Instance;
        mainCamera = Camera.main;

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(0.3f);

        Debug.Log("spawning now!");

        for (int w = 0; w < waves.Count; w++)
        {
            var wave = waves[w];

            for (int g = 0; g < wave.SpawnGroups.Count; g++)
            {
                var group = wave.SpawnGroups[g];

                for (int i = 0; i < group.SpawnAmount; i++)
                {
                    var spawnPos = transform.position + new Vector3(side * (0.5f - gapFromEdge), 0);

                    //@TODO also need to reuse enemy prefabs rather than continually spawning new ones
                    var newEnemy = Instantiate(group.EnemyPrefab, spawnPos, transform.rotation, transform).GetComponent<Enemy>();
                    var pathSide = EnemyInfo.PathSide.left;
                    if (side == 1) pathSide = EnemyInfo.PathSide.right;

                    newEnemy.Initialize(pathSide, new Angle(transform.rotation), mainCamera);

                    enemyController.Add(newEnemy);

                    side = -side;
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}
