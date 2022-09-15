using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveInterface : Singleton<WaveInterface>
{
    // set in inspector
    [SerializeField] StartWave startWave;
    public Transform waveScroller;

    // references
    WaveController _waveController;

    private void Start()
    {
        _waveController = WaveController.Instance;
        _waveController.onSpawningChange.AddListener(OnSpawningChange);

        startWave.button.onClick.AddListener(OnStartWaveClick);
    }

    private void OnStartWaveClick()
    {
        _waveController.SpawnNextWave();
    }

    private void OnSpawningChange(bool spawning)
    {
        startWave.button.interactable = !spawning;
        if (spawning)
        {
            startWave.button.OnHoverEnd();
        }
    }
}
