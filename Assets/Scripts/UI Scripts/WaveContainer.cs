using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// For the wave container prefab - contains all info regarding one wave container
/// </summary>
public class WaveContainer : MonoBehaviour
{
    const int MAX_ENEMY_CONTAINERS = 5;

    // set in inspector
    public TextMeshProUGUI waveNumberText;
    public Transform rightContainer; // where enemy containers are spawned

    // prefab references
    public EnemyContainer enemyContainerPrefab;

    // variables
    public List<EnemyContainer> enemyContainers = new List<EnemyContainer>();

    private void Awake()
    {
        // pre-spawn enemy containers
        for (int i = 0; i < MAX_ENEMY_CONTAINERS; i++)
        {
            EnemyContainer newContainer = Instantiate(enemyContainerPrefab, rightContainer);
            newContainer.gameObject.SetActive(false);
            enemyContainers.Add(newContainer);
        }
    }

    public void RecreateEnemyContainers(WaveInfo wave, int waveIndex)
    {
        // apply the wave number
        waveNumberText.text = (waveIndex + 1).ToString();
        // first clear enemy containers
        foreach (EnemyContainer container in enemyContainers)
        {
            container.Clear();
            container.gameObject.SetActive(false);
        }

        int containerIndex = 0;
        for (int i = 0; i < wave.spawnGroups.Count; i++)
        {
            SpawnGroup group = wave.spawnGroups[i];
            EnemyInfo info = group.enemyPrefab.info;

            // first search through existing containers to see if we can add it
            EnemyContainer matchingContainer = enemyContainers.Find(x => x.currentInfo == info);
            if (matchingContainer)
            {
                matchingContainer.AddAmount(group.spawnAmount);
                continue;
            }

            // make sure we still have space for a new enemy type
            if (containerIndex >= MAX_ENEMY_CONTAINERS)
            {
                Debug.LogError($"More than {MAX_ENEMY_CONTAINERS} enemy types in wave {waveIndex + 1}, cannot render!");
                return;
            }


            // add a new container
            EnemyContainer container = enemyContainers[containerIndex];
            container.gameObject.SetActive(true);
            container.currentInfo = info;
            container.enemyImage.sprite = info.previewSprite;
            container.AddAmount(group.spawnAmount);

            // now add to the index to indicate we have used this container
            containerIndex++;
        }
    }
}