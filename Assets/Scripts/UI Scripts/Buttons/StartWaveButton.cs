using DG.Tweening;
using UnityEngine;

public class StartWaveButton : UIButton
{
    public override void OnClick()
    {
        WaveController.Instance.SpawnNextWave();
        WaveInterface.Instance.ScrollToNextWave();
        interactable = false;
        OnHoverEnd();
    }

    public override void OnHoverStart()
    {
        transform.DOScale(new Vector2(0.9f, 0.9f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        transform.DOScale(new Vector2(1, 1), 0.1f);
    }
}