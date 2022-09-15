using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    // set in inspector
    public float maximumOffset = 0;
    public float timeToFirstWave = 5;
    public List<WaveInfo> waves = new List<WaveInfo>();
    public PathPoint firstPoint;

    // internal variables
    bool spawning = false;
    int currentWaveIndex = 0;

    EnemyController _enemyController;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = EnemyController.Instance;
    }

    public void SpawnNextWave()
    {
        if (spawning)
        {
            Debug.LogError("Should not be able to start next wave whilst spawning!");
            return;
        }

        StartCoroutine(SpawnWave(currentWaveIndex));
    }

    private IEnumerator SpawnWave(int waveIndex)
    {
        if (waves.Count <= waveIndex)
        {
            Debug.LogError("Trying to spawn wave which does not exist, at index " + waveIndex);
            yield break;
        }

        spawning = true;
        WaveInfo wave = waves[waveIndex];

        for (int g = 0; g < wave.spawnGroups.Count; g++)
        {
            var group = wave.spawnGroups[g];

            // wait spawning delay
            yield return new WaitForSeconds(group.startingDelay);

            for (int i = 0; i < group.spawnAmount; i++)
            {
                // actually spawn, this is a mess at the moment
                float offset = ChooseRandomOffset();
                Angle angleToFirstPoint = Angle.Towards(transform.position, firstPoint.transform.position);

                Vector2 spawnPos = (Vector2)transform.position + (angleToFirstPoint.Clone().Rotate(270).AsVector() * offset);


                //Enemy newEnemy = _enemyController.SpawnEnemy(
                //    spawnPos,
                //    angleToFirstPoint,
                //    group.enemyPrefab,
                //    firstPoint
                //);

                yield return new WaitForSeconds(group.delayBetweenSpawns);
            }
        }

        // we have finished spawning, allow starting next wave
        spawning = false;
        currentWaveIndex++;
        //@todo grey out starting button when spawning?
    }

    private float ChooseRandomOffset()
    {
        float side = Mathf.Sign(Random.Range(-1, 1));
        float finalOffset = Random.value * side * maximumOffset;
        return finalOffset;
    }
}
