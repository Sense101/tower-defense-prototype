using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WaveInterface : Singleton<WaveInterface>
{
    // set in inspector
    [SerializeField] StartWaveButton startWaveButton;
    [SerializeField] TextMeshProUGUI startWaveText;
    [SerializeField] RectTransform innerContainer;

    // internal variables
    public List<WaveContainer> containers;

    // references

    private void Start()
    {
        containers = new List<WaveContainer>(GetComponentsInChildren<WaveContainer>());
    }

    public async void ScrollToNextWave()
    {
        // start moving the second container into place
        containers[1].transform.DOMoveX(innerContainer.position.x, 2);

        // move the first container away
        await containers[0].transform.DOMoveX(innerContainer.position.x - innerContainer.sizeDelta.x, 2).AsyncWaitForCompletion();

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
        startWaveButton.interactable = true;
    }
}
