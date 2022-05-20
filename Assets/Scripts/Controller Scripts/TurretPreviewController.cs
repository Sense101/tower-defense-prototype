using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretPreviewController : Singleton<TurretPreviewController>
{
    const string ANIMATOR_HIDE = "hide";
    const float FADE_TIME_MULTIPLIER = 1;

    // set in inspector
    [SerializeField] CanvasFadeGroup fadeGroup = default;
    [SerializeField] Image basePreview = default;
    [SerializeField] Image mainPreview = default;

    // internal variables
    Turret _currentPrefab = null;
    Coroutine _currentCoroutine = null;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void SetPreview(Turret turretPrefab)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(RerenderPreview(turretPrefab));
    }

    private IEnumerator RerenderPreview(Turret turretPrefab)
    {
        // start hiding
        // wait till hidden
        // change sprites
        //
        if (_currentPrefab?.info != turretPrefab.info)
        {
            // it's different, rerender fully
            _currentPrefab = turretPrefab;

            fadeGroup.Hide();
            yield return new WaitUntil(() => fadeGroup.state == CanvasFadeGroup.FadeState.hidden);

            // change the sprites
            basePreview.sprite = turretPrefab.info.baseSprite;
            mainPreview.sprite = turretPrefab.info.previewSprite;
        }

        fadeGroup.Show();
        yield return new WaitUntil(() => fadeGroup.state == CanvasFadeGroup.FadeState.shown);

        // coroutine is done
        _currentCoroutine = null;
    }
}
