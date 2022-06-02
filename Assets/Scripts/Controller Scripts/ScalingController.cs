using UnityEngine;

// automatically scales the camera so it stays in view of the screen, and scales the canvas with it
//@TODO merge into UI controller??
public class ScalingController : Singleton<ScalingController>
{

    // more than ideal ratio = wider, less = taller
    const float IDEAL_RATIO = 16f / 9f;
    float desiredCameraHeight;
    float desiredCameraWidth;

    float _currentRatio = 0;

    Camera _camera;
    RectTransform _canvasTransform;
    UIController _uiController;

    private void Start()
    {
        _camera = Camera.main;
        desiredCameraHeight = _camera.orthographicSize * 2;
        desiredCameraWidth = desiredCameraHeight * IDEAL_RATIO;

        _canvasTransform = GameObject.FindWithTag(UIController.CANVAS_TAG).GetComponent<RectTransform>();
        _uiController = UIController.Instance;
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
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

        //float currentCameraWidth = cameraHeight * _currentRatio;
        //_camera.transform.position = new Vector3
        //(
        //    8 + ((desiredCameraWidth - currentCameraWidth) / 2),
        //    4.5f + ((desiredCameraHeight - cameraHeight) / 2),
        //    -10
        //);

        // scale the UI parts - we have to find them every time because it runs in the editor
        foreach (UIElement e in _uiController.Elements)
        {
            Vector2 defaultSize = e.scale.defaultSize;

            // find the rect transform to scale from, default is canvas
            Vector2 baseSize = e.baseElement.RectTransform.sizeDelta;
            Vector2 baseDefaultSize;
            if (e.scalingBase == UIElement.ScalingBase.canvas)
            {
                baseDefaultSize = new Vector2(desiredCameraWidth, desiredCameraHeight);
            }
            else
            {
                baseDefaultSize = e.baseElement.scale.defaultSize;
            }

            // difference in world units
            Vector2 baseSizeDiff = baseSize - baseDefaultSize;

            // scale size
            if (e.scale.active)
            {
                float newWidth = defaultSize.x + (baseSizeDiff.x * e.scale.widthIncrease);
                float newHeight = defaultSize.y + (baseSizeDiff.y * e.scale.heightIncrease);

                e.RectTransform.sizeDelta = new Vector2(newWidth, newHeight);
            }

            //----
            // POSITION
            //----

            if (e.position.active)
            {
                e.RectTransform.anchoredPosition = new Vector2
                    (
                        e.position.DefaultPosition.x + (baseSizeDiff.x * e.position.XMovement),
                        e.position.DefaultPosition.y + (baseSizeDiff.y * e.position.YMovement)
                    );
            }
        }
    }
}