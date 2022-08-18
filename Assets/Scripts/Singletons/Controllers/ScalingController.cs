using UnityEngine;

// automatically scales the camera so it stays in view of the screen, and scales the canvas with it
//@TODO merge into UI controller??
public class ScalingController : Singleton<ScalingController>
{

    // more than ideal ratio = wider, less = taller
    const float IDEAL_RATIO = 16f / 9f;
    const string CANVAS_TAG = "MainCanvas";

    float desiredCameraHeight;
    float desiredCameraWidth;

    float _currentRatio = 0;

    Camera _camera;
    RectTransform _canvasTransform;

    private void Start()
    {
        _camera = Camera.main;
        desiredCameraHeight = _camera.orthographicSize * 2;
        desiredCameraWidth = desiredCameraHeight * IDEAL_RATIO;

        _canvasTransform = GameObject.FindWithTag(CANVAS_TAG).GetComponent<RectTransform>();
    }

    // Adjust the camera's height so the desired scene width fits in view
    // - even if the screen/window size changes dynamically.
    private void Update()
    {
        // check if the scale has changed
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
            _camera.orthographicSize = desiredCameraHeight / 2;
        }
        else
        {
            float unitsPerPixel = desiredCameraWidth / Screen.width;
            float desiredHeight = unitsPerPixel * Screen.height;

            _camera.orthographicSize = Mathf.Max(desiredHeight, desiredCameraHeight) / 2;
        }

        // scale the canvas
        float cameraHeight = _camera.orthographicSize * 2;
        _canvasTransform.sizeDelta = new Vector2(cameraHeight * _currentRatio, cameraHeight);
    }
}