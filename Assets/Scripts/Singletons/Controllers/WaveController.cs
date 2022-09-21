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
    public bool safeGapToSpawn = true;
    public int currentWaveIndex = 0;

    EnemyController _enemyController;
    WaveInterface _waveInterface;

    // Start is called before the first frame update
    private void Start()
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

            // wait until there is a safe gap to spawn
            yield return new WaitUntil(() => safeGapToSpawn);

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
                    group.enemyPrefab.gameObject,
                    firstPoint.nextPoint,
                    firstPoint.GetNextPosition(offset),
                    offset
                );

                if (i + 1 < group.spawnAmount)
                {
                    yield return new WaitForSeconds(group.delayBetweenSpawns);
                }
                else
                {
                    safeGapToSpawn = false;
                    StartCoroutine(CreateSafeSpawningGap(group.delayBetweenSpawns));
                }
            }
        }

        // wait for one second before allowing next wave to spawn
        yield return new WaitForSeconds(1f);

        // we have finished spawning, allow starting next wave
        spawning = false;

        // only re-enable button if there are more waves
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            _waveInterface.EnableStartWaveButton();
        }
    }

    // this exists so the player can press start wave as soon as this waves enemies have spawned
    private IEnumerator CreateSafeSpawningGap(float time)
    {
        yield return new WaitForSeconds(time);
        safeGapToSpawn = true;
    }

    private float ChooseRandomOffset()
    {
        float side = Mathf.Sign(Random.Range(-1, 1));
        float finalOffset = Random.value * side * firstPoint.maximumOffset;
        return finalOffset;
    }
}
