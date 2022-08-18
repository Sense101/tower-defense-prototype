using UnityEngine;

public class AugmentInterface : Singleton<AugmentInterface>
{
    // references
    public CanvasFadeGroup fadeGroup;

    private void Start()
    {
        // set references
        fadeGroup = GetComponent<CanvasFadeGroup>();
    }

    public void Show()
    {
        fadeGroup.Show();
    }

    public void Hide()
    {
        fadeGroup.Hide();
    }
}
