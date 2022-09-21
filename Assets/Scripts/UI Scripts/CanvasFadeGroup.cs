using UnityEngine;

/// <summary>
/// script to manage hiding and showing canvas groups
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CanvasFadeGroup : MonoBehaviour
{
    public enum FadeState { hiding, showing, hidden, shown }
    public FadeState state = FadeState.hidden;

    // accessors for checking the state quickly
    public bool Hiding { get => state == FadeState.hiding; }
    public bool Showing { get => state == FadeState.showing; }
    public bool Hidden { get => state == FadeState.hidden; }
    public bool Shown { get => state == FadeState.shown; }

    [SerializeField] float fadeTime = 1;
    [SerializeField] bool useUnscaledTime = true;

    [Header("Scaling")]
    [SerializeField] bool modifyScale = false;
    [SerializeField] float hiddenScale = 1;
    [SerializeField] float shownScale = 1;

    [Header("Position")]
    [SerializeField] bool modifyPosition = false;
    [SerializeField] bool relativeToStartPosition = false;
    [SerializeField] Vector2 hiddenPosition = Vector2.zero;
    [SerializeField] Vector2 shownPosition = Vector2.zero;

    float _cachedhiddenToShownScale;
    float _cachedhiddenToShownDistance;

    CanvasGroup _group;
    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();

        // cache
        if (modifyScale)
        {
            _cachedhiddenToShownScale = shownScale - hiddenScale;
        }
        if (modifyPosition)
        {
            if (relativeToStartPosition)
            {
                // change positions to be relative
                hiddenPosition += (Vector2)transform.localPosition;
                shownPosition += (Vector2)transform.localPosition;
            }

            _cachedhiddenToShownDistance = Vector2.Distance(hiddenPosition, shownPosition);
        }

        // update the group immediately to match the state
        if (Hidden || Showing)
        {
            CompleteHide();
        }
        else if (Shown || Hiding)
        {
            CompleteShow();
        }
    }

    private void Update()
    {
        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        // do stuff with canvas group depending on state
        if (Hiding)
        {
            // slowly hide
            if (_group.alpha > 0)
            {
                _group.alpha -= (1 / fadeTime) * deltaTime;

                ModifyScale(_group.alpha);
                ModifyPosition(_group.alpha);
            }
            else
            {
                state = FadeState.hidden;
                CompleteHide();
            }
        }
        else if (Showing)
        {
            // slowly show
            if (_group.alpha < 1)
            {
                _group.alpha += (1 / fadeTime) * deltaTime;

                ModifyScale(_group.alpha);
                ModifyPosition(_group.alpha);
            }
            else
            {
                state = FadeState.shown;
                CompleteShow();
            }
        }
    }

    public void Hide(bool forceInstant = false)
    {
        if (Shown || Showing)
        {
            // disable interacting as soon as we start hiding
            _group.interactable = false;
            _group.blocksRaycasts = false;

            if (fadeTime <= 0 || forceInstant)
            {
                // it's instant
                _group.alpha = 0;
                state = FadeState.hidden;
            }
            else
            {
                // set the state so we can hide over time
                state = FadeState.hiding;
            }
        }
    }

    public void Show(bool forceInstant = false)
    {
        // enable interacting as soon as we start showing
        _group.interactable = true;
        _group.blocksRaycasts = true;

        if (Hidden || Hiding)
        {
            if (fadeTime <= 0 || forceInstant)
            {
                // it's instant
                state = FadeState.shown;
            }
            else
            {
                // set the state so we can show over time
                state = FadeState.showing;
            }
        }
    }

    private void ModifyScale(float alpha)
    {
        if (modifyScale)
        {
            float newScale = hiddenScale + (_cachedhiddenToShownScale * _group.alpha);
            transform.localScale = new Vector2(newScale, newScale);
        }
    }

    private void ModifyPosition(float alpha)
    {
        if (modifyPosition)
        {
            float newDistance = _cachedhiddenToShownDistance * _group.alpha;
            transform.localPosition = Vector2.MoveTowards(hiddenPosition, shownPosition, newDistance);
        }
    }

    private void CompleteHide()
    {
        _group.alpha = 0;
        _group.interactable = false;
        _group.blocksRaycasts = false;

        if (modifyScale)
        {
            transform.localScale = new Vector2(hiddenScale, hiddenScale);
        }
        if (modifyPosition)
        {
            transform.localPosition = hiddenPosition;
        }
    }

    private void CompleteShow()
    {
        _group.alpha = 1;
        _group.interactable = true;
        _group.blocksRaycasts = true;

        if (modifyScale)
        {
            transform.localScale = new Vector2(shownScale, shownScale);
        }
        if (modifyPosition)
        {
            transform.localPosition = shownPosition;
        }
    }
}
