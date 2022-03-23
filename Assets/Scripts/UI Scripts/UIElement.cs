using UnityEngine;

// checks for a "button" in the editor which sets the width and height
[ExecuteInEditMode]
public class UIElement : MonoBehaviour
{
    // scaling
    [Space(5)]
    [Tooltip("the rect transform that the scale is based off (defaults to the main canvas)")]
    public RectTransform BaseTransform = null;

    [Space(5)]
    [Tooltip("the desired width if the ratio is 16:9")]
    public float BaseWidth = 1;
    public bool ScaleWidth = false;

    [Space(5)]
    [Tooltip("the desired height if the ratio is 16:9")]
    public float BaseHeight = 1;
    public bool ScaleHeight = false;

    [Space(5)]
    [Tooltip("sets the base width/height to the current size")]
    [SerializeField] bool DetectSize = false; // custom button

    // references

    // set by scaling controller
    [HideInInspector] public RectTransform _rectTransform;

    private void Awake()
    {
        // @TODO work out if this ever needs to be set still
        _rectTransform = GetComponent<RectTransform>();

        // add the element to the controller, let it do the rest
        UIController.Instance?._elements.Add(this);
    }

    private void Update()
    {
        if (DetectSize)
        {
            DetectSize = false;
            BaseWidth = _rectTransform.sizeDelta.x;
            BaseHeight = _rectTransform.sizeDelta.y;
        }
    }

    public virtual void UpdateCustomScaling(Vector2 baseSize)
    {
        // called every time the scale changes, can be overridden to create custom logic
        return;
    }
}
