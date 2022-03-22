using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    static readonly string fadeInTrigger = "healthBarFadeIn";

    [SerializeField] Canvas canvas = default;
    [SerializeField] Image innerHealthBar = default;

    Animator animator;

    bool visible = false;

    // makes sure the canvas has the camera assigned
    public void Initialize(Camera camera)
    {
        canvas.worldCamera = camera;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// set the fill amount of the health bar between 0 and 1
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetFill(float fillAmount)
    {
        if (!visible)
        {
            visible = true;
            animator.SetTrigger(fadeInTrigger);
        }

        innerHealthBar.fillAmount = fillAmount;
    }
}
