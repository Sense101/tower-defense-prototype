using UnityEngine;

// automatically scales the camera so it stays in view of the screen, and scales the canvas with it
[ExecuteInEditMode]
public class ScalingController : Singleton<ScalingController>
{
    // tags
    const string CANVAS_TAG = "MainCanvas";

    // more than ideal ratio = wider, less = taller
    const float IDEAL_RATIO = 16f / 9f;
    const float DESIRED_CAMERA_SIZE = 5f;
    const float DESIRED_CANVAS_WIDTH = 18f;

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
            _camera.orthographicSize = DESIRED_CAMERA_SIZE;
        }
        else
        {
            float unitsPerPixel = 17.77f / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = Mathf.Max(desiredHalfHeight, DESIRED_CAMERA_SIZE);
        }

        // scale the canvas
        float cameraHeight = _camera.orthographicSize * 2;
        _canvasTransform.sizeDelta = new Vector2(cameraHeight * _currentRatio, cameraHeight);

        // scale the UI
        // NOTE: we can't use the UI controller cache as this is not during runtime
        foreach (UIElement e in FindObjectsOfType<UIElement>())
        {
            e._rectTransform = e.GetComponent<RectTransform>();
            Vector2 oldSize = e._rectTransform.sizeDelta;

            // find the rect transform to scale from, default is canvas
            Vector2 baseSize = e.RelativeTransform
                ? e.RelativeTransform.sizeDelta
                : _canvasTransform.sizeDelta;

            // scale width
            float newWidth = oldSize.x;
            if (e.ScaleWidth)
            {
                newWidth = e.BaseWidth * _currentRatio / IDEAL_RATIO;
            }

            // scale height
            float newHeight = oldSize.y;
            if (e.ScaleHeight)
            {
                newHeight = e.BaseHeight * _camera.orthographicSize / DESIRED_CAMERA_SIZE;
            }

            e._rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

            // call custom scaling code
            e.UpdateCustomScaling(baseSize);
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