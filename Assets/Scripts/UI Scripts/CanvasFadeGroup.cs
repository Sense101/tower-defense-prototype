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

    // set in inspector
    [SerializeField] float fadeTime = 1;

    CanvasGroup _group;
    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();

        // update the group immediately to match the state
        if (Hidden)
        {
            _group.alpha = 0;
            _group.interactable = false;
            _group.blocksRaycasts = false;
        }
        else if (Shown)
        {
            _group.alpha = 1;
            _group.interactable = true;
            _group.blocksRaycasts = true;
        }
    }

    private void Update()
    {
        // do stuff with canvas group depending on state
        if (Hiding)
        {
            // slowly hide
            if (_group.alpha > 0)
            {
                _group.alpha -= (1 / fadeTime) * Time.deltaTime;
            }
            else
            {
                state = FadeState.hidden;
            }
        }
        else if (Showing)
        {
            // slowly show
            if (_group.alpha < 1)
            {
                _group.alpha += (1 / fadeTime) * Time.deltaTime;
            }
            else
            {
                state = FadeState.shown;

                // reenable interaction when we are fully visible
                _group.interactable = true;
                _group.blocksRaycasts = true;
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
        if (Hidden || Hiding)
        {
            if (fadeTime <= 0 || forceInstant)
            {
                // it's instant
                _group.alpha = 1;
                state = FadeState.shown;
            }
            else
            {
                // set the state so we can show over time
                state = FadeState.showing;
            }
        }
    }
}
