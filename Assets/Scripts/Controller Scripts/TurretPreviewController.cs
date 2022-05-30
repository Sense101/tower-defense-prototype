using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretPreviewController : Singleton<TurretPreviewController>
{

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
        // main preview to turn towards mouse
        if (_currentPrefab)
        {
            //@TODO only point if mouse is not within box - aka within x+y of us

            // find the angle from the preview to the mouse
            Vector2 mousePos = InputController.mousePos;

            bool withinX = Mathf.Abs(mousePos.x - transform.position.x) <= 1;
            bool withinY = Mathf.Abs(mousePos.y - transform.position.y) <= 1;
            if (withinX && withinY)
            {
                return;
            }

            Angle angleToMouse = Angle.Towards(transform.position, mousePos);

            // turn the turret preview toards the mouse
            mainPreview.transform.rotation = angleToMouse.AsQuaternion();
            Quaternion.RotateTowards
            (
                mainPreview.transform.rotation, // from
                angleToMouse.AsQuaternion(), // to
                _currentPrefab.info.SpinSpeed / 2 * Time.deltaTime // delta speed
            );
        }
    }



    public void SetPreview(Turret turretPrefab)
    {
        // stop the old coroutine
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // only rerender if it's a valid turret
        if (turretPrefab)
        {
            _currentCoroutine = StartCoroutine(RerenderPreview(_currentPrefab, turretPrefab));
        }

        _currentPrefab = turretPrefab;
    }

    private IEnumerator RerenderPreview(Turret oldPrefab, Turret newPrefab)
    {
        // render instantly if we had nothing till now
        bool renderInstantly = !oldPrefab;

        if (oldPrefab?.info != newPrefab.info)
        {

            // it's different, rerender fully
            fadeGroup.Hide(renderInstantly);

            yield return new WaitUntil(() => fadeGroup.state == CanvasFadeGroup.FadeState.hidden);

            // change the sprites
            basePreview.sprite = newPrefab.info.baseSprite;
            mainPreview.sprite = newPrefab.info.previewSprite;

            _currentPrefab = newPrefab;
        }

        fadeGroup.Show(renderInstantly);
        yield return new WaitUntil(() => fadeGroup.state == CanvasFadeGroup.FadeState.shown);

        // coroutine is done
        _currentCoroutine = null;
    }
}
