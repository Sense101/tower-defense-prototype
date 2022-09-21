using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartWaveButton : UIButton
{
    private Transform _innerImage; // @todo this issue needs a proper resolution

    protected override void SetReferences()
    {
        _innerImage = transform.GetChild(0);
    }

    public override void OnClick()
    {
        interactable = false;
        WaveController.Instance.SpawnNextWave();
        WaveInterface.Instance.ScrollToNextWave();

        OnHoverEnd();
    }

    public override void OnHoverStart()
    {
        _innerImage.DOScale(new Vector2(0.9f, 0.9f), 0.1f);
    }

    public override void OnHoverEnd()
    {
        _innerImage.DOScale(new Vector2(1, 1), 0.1f);
    }
}