using UnityEngine;

/// <summary>
/// script to manage hiding and showing canvas groups
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CanvasFadeGroup : MonoBehaviour
{
    public enum FadeState { hiding, showing, hidden, shown }
    public FadeState state = FadeState.hidden;

    // set in inspector
    [SerializeField] float fadeTime = 1;

    CanvasGroup _group;
    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();

        // update the group immediately to match the state
        if (state == FadeState.hidden)
        {
            _group.alpha = 0;
        }
        else if (state == FadeState.shown)
        {
            _group.alpha = 1;
        }
    }

    private void Update()
    {
        // do stuff with canvas group depending on state
        if (state == FadeState.hiding)
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
        else if (state == FadeState.showing)
        {
            // slowly show
            if (_group.alpha < 1)
            {
                _group.alpha += (1 / fadeTime) * Time.deltaTime;
            }
            else
            {
                state = FadeState.shown;
            }
        }
    }

    public void Hide(bool forceInstant = false)
    {
        if (state == FadeState.shown || state == FadeState.showing)
        {

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
        if (state == FadeState.hidden || state == FadeState.hiding)
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
