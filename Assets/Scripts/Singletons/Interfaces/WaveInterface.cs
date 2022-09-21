using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WaveInterface : Singleton<WaveInterface>
{
    public const float ANIMATION_TIME = 2;
    // set in inspector
    [SerializeField] StartWaveButton startWaveButton;
    [SerializeField] TextMeshProUGUI startWaveText;
    [SerializeField] RectTransform innerContainer;

    // internal variables
    public List<WaveContainer> containers;

    // references
    WaveController _waveController;

    private void Start()
    {
        _waveController = WaveController.Instance;

        containers = new List<WaveContainer>(GetComponentsInChildren<WaveContainer>());

        // recreate enemy containers for first wave container
        containers[0].RecreateEnemyContainers(
            _waveController.waves[_waveController.currentWaveIndex],
            _waveController.currentWaveIndex
        );
    }

    public async void ScrollToNextWave()
    {
        // recreate the enemy containers for the second container
        int nextWaveIndex = _waveController.currentWaveIndex + 1;
        if (nextWaveIndex < _waveController.waves.Count)
        {
            containers[1].RecreateEnemyContainers(
                _waveController.waves[nextWaveIndex],
                nextWaveIndex
            );

            // start moving the second container into place
            containers[1].transform.DOMoveX(innerContainer.position.x, ANIMATION_TIME);
        }
        else
        {
            // hide the next container completely
            containers[1].gameObject.SetActive(false);
        }


        // wait for the first container to move away
        await containers[0].transform.DOMoveX(innerContainer.position.x - innerContainer.sizeDelta.x, ANIMATION_TIME).AsyncWaitForCompletion();

        if (!containers[0])
        {
            // the scene has since been changed
            return;
        }

        // now move the first container to the back of the line
        // @todo and update it to match the next wave
        containers[0].transform.DOMoveX(innerContainer.position.x + innerContainer.sizeDelta.x, 0);
        containers.Add(containers[0]);
        containers.RemoveAt(0);
    }

    private async void ResetOldWaveContainer(System.Threading.Tasks.Task movementTask)
    {
        await movementTask;
        containers[0].transform.position += new Vector3(88, 0);
        containers.Add(containers[0]);
        containers.RemoveAt(0);
    }

    public void EnableStartWaveButton()
    {
        if (startWaveButton.hovering)
        {
            startWaveButton.OnHoverStart();
        }
        startWaveButton.interactable = true;
    }
}
