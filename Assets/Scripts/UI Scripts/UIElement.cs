using System.Collections.Generic;
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
    [Tooltip("sets the base width/height to the current size")]
    [SerializeField] bool DetectSize = false; // custom button

    private Vector2 _cachedBaseSize = Vector2.zero;

    public Vector2 CachedBaseSize
    {
        get
        {
            //@TODO got to fix this back to what it was
            return _cachedBaseSize;
        }
    }

    [Space(5)]
    public ElementScale Scale;

    public List<ElementPart> Parts = new List<ElementPart>();

    // references
    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    private void Update()
    {
        if (DetectSize)
        {
            DetectSize = false;
            Scale.DefaultSize = _rectTransform.sizeDelta;
        }
    }

    public virtual void UpdateCustomScaling(Vector2 baseSize)
    {
        // called every time the scale changes, can be overridden to create custom logic
        return;
    }
}
