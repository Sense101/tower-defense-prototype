using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class CanvasFadeGroup : MonoBehaviour
{
    const string FADE_TIME_MULTIPLIER = "fadeTimeMultiplier";
    public enum FadeState { hiding, showing, hidden, shown }

    // @TODO for now, start invisible always
    public FadeState state = FadeState.hidden;

    // set in inspector
    [SerializeField] float showingTime = 1;
    [SerializeField] float hidingTime = 1;

    Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        switch (state)
        {
            case FadeState.hiding:
            case FadeState.hidden:
                return;
            case FadeState.shown:
                break;
            case FadeState.showing:
                // quick switch
                state = FadeState.hiding;
                break;
        }
        _animator.SetFloat(FADE_TIME_MULTIPLIER, -1 / hidingTime);
    }

    public void Show()
    {
        switch (state)
        {
            case FadeState.showing:
            case FadeState.shown:
                return;
            case FadeState.hidden:
                break;
            case FadeState.hiding:
                // quick switch
                state = FadeState.showing;
                break;
        }
        _animator.SetFloat(FADE_TIME_MULTIPLIER, 1 / showingTime);
    }

    // called at the beginning and end of the animation
    public void AnimatorPause()
    {
        switch (state)
        {
            case FadeState.hiding:
                state = FadeState.hidden;
                break;
            case FadeState.showing:
                state = FadeState.shown;
                break;
            case FadeState.hidden:
                state = FadeState.showing;
                return;
            case FadeState.shown:
                state = FadeState.hiding;
                return;
        }
        // actually pause
        _animator.SetFloat(FADE_TIME_MULTIPLIER, 0);
    }
}
