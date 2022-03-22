using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MatchWidth : MonoBehaviour
{
    // more than ideal ratio = wider, less = taller
    static float IDEAL_RATIO = 16f / 9f;
    static float DESIRED_CAMERA_SIZE = 5f;

    float _currentRatio = 0;

    Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        float newRatio = (float)Screen.width / (float)Screen.height;
        if (newRatio == _currentRatio)
        {
            return;
        }
        _currentRatio = newRatio;

        if (_currentRatio >= IDEAL_RATIO)
        {
            _camera.orthographicSize = DESIRED_CAMERA_SIZE;
        }
        else
        {
            float unitsPerPixel = 18f / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = Mathf.Max(desiredHalfHeight, DESIRED_CAMERA_SIZE);
        }
    }
}