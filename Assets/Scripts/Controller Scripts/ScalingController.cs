using UnityEngine;

// automatically scales the camera so it stays in view of the screen, and scales the canvas with it
[ExecuteInEditMode]
public class ScalingController : Singleton<ScalingController>
{
    // tags
    const string CANVAS_TAG = "MainCanvas";

    // more than ideal ratio = wider, less = taller
    const float IDEAL_RATIO = 16f / 9f;
    const float DESIRED_CAMERA_HEIGHT = 10;
    const float DESIRED_CAMERA_WIDTH = DESIRED_CAMERA_HEIGHT * IDEAL_RATIO;

    float _currentRatio = 0;

    Camera _camera;
    RectTransform _canvasTransform;

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    private void Update()
    {
        SetReferences();

        // check if the scale has changed, if so recalculate
        float newRatio = (float)Screen.width / (float)Screen.height;
        if (newRatio == _currentRatio)
        {
            // don't rescale, everything is the same
            return;
        }
        _currentRatio = newRatio;

        // scale the camera
        if (_currentRatio > IDEAL_RATIO || Mathf.Approximately(_currentRatio, IDEAL_RATIO))
        {
            _camera.orthographicSize = DESIRED_CAMERA_HEIGHT / 2;
        }
        else
        {
            float unitsPerPixel = DESIRED_CAMERA_WIDTH / Screen.width;
            float desiredHeight = unitsPerPixel * Screen.height;

            _camera.orthographicSize = Mathf.Max(desiredHeight, DESIRED_CAMERA_HEIGHT) / 2;
        }

        // scale the canvas
        float cameraHeight = _camera.orthographicSize * 2;
        _canvasTransform.sizeDelta = new Vector2(cameraHeight * _currentRatio, cameraHeight);

        // scale the UI parts - we have to find them every time because it runs in the editor
        foreach (UIElement e in FindObjectsOfType<UIElement>())
        {
            //----
            // SCALE
            //----

            Vector2 defaultSize = e.Scale.DefaultSize;

            // find the rect transform to scale from, default is canvas
            Vector2 baseSize = e.BaseTransform
                ? e.BaseTransform.sizeDelta
                : _canvasTransform.sizeDelta;

            Vector2 baseDefaultSize = new Vector2(DESIRED_CAMERA_WIDTH, DESIRED_CAMERA_HEIGHT);
            if (e.BaseTransform)
            {
                // check for a UI element with a default size
                UIElement baseElement = null;
                e.BaseTransform.TryGetComponent<UIElement>(out baseElement);
                baseDefaultSize = baseElement
                    ? baseElement.Scale.DefaultSize
                    : e.BaseTransform.sizeDelta;
            }


            // difference in world units
            Vector2 baseSizeDiff = baseSize - baseDefaultSize;

            // scale size
            float newWidth = defaultSize.x + (baseSizeDiff.x * e.Scale.WidthIncrease);
            float newHeight = defaultSize.y + (baseSizeDiff.y * e.Scale.HeightIncrease);

            e.RectTransform.sizeDelta = new Vector2(newWidth, newHeight);

            //----
            // POSITION
            //----

            ElementPosition position = e.Parts.Find(x => x is ElementPosition) as ElementPosition;
            if (position)
            {
                e.RectTransform.anchoredPosition = new Vector2
                (
                    position.DefaultPosition.x + (baseSizeDiff.x * position.XMovement),
                    position.DefaultPosition.y + (baseSizeDiff.y * position.YMovement)
                );
            }
        }
    }

    // set references, if they don't yet exist
    private void SetReferences()
    {
        // camera
        if (!_camera)
        {
            _camera = Camera.main;
        }
        // canvas
        if (!_canvasTransform)
        {
            _canvasTransform = GameObject.FindWithTag(CANVAS_TAG).GetComponent<RectTransform>();
        }
    }
}