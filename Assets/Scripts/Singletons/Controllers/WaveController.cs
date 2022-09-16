using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : Singleton<WaveController>
{
    // set in inspector
    public List<WaveInfo> waves = new List<WaveInfo>();
    public PathPoint firstPoint;

    // variables
    public bool spawning = false;
    public int currentWaveIndex = 0;
    public UnityEvent<bool> onSpawningChange = new UnityEvent<bool>();

    EnemyController _enemyController;
    WaveInterface _waveInterface;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = EnemyController.Instance;
        _waveInterface = WaveInterface.Instance;
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
                Angle angleToNextPoint = Angle.Towards(firstPoint.transform.position, firstPoint.nextPoint.transform.position);

                Vector2 spawnPos = (Vector2)firstPoint.transform.position + (angleToNextPoint.CloneRotate(-90).AsVector() * offset);


                Enemy newEnemy = _enemyController.SpawnEnemy(
                    spawnPos,
                    angleToNextPoint,
                    group.enemyPrefab,
                    firstPoint.nextPoint,
                    firstPoint.GetNextPosition(offset),
                    offset
                );

                yield return new WaitForSeconds(group.delayBetweenSpawns);
            }
        }

        // we have finished spawning, allow starting next wave
        spawning = false;
        _waveInterface.EnableStartWaveButton();
        currentWaveIndex++;
    }

    private float ChooseRandomOffset()
    {
        float side = Mathf.Sign(Random.Range(-1, 1));
        float finalOffset = Random.value * side * firstPoint.maximumOffset;
        return finalOffset;
    }
}
