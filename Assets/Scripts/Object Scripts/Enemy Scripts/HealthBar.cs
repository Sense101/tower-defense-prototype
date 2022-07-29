using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    const string FADE_IN_TRIGGER = "healthBarFadeIn";

    [SerializeField] Canvas canvas;
    [SerializeField] SpriteRenderer outerBar;
    [SerializeField] Image innerHealthBar;
    [SerializeField] Image innerArmorBar;

    Animator animator;

    bool visible = false;

    public void SetReferences(Camera camera)
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

            StartCoroutine(Show());
        }

        innerHealthBar.fillAmount = fillAmount;
    }

    private IEnumerator Show()
    {
        Color currentColor = new Color(1, 1, 1, 0);
        while (currentColor.a < 1)
        {
            currentColor.a += Time.deltaTime;

            // set visiblities
            outerBar.color = currentColor;
            innerHealthBar.color = currentColor;

            yield return null;
        }
    }

    public void Hide()
    {
        outerBar.color = Color.clear;
        innerHealthBar.color = Color.clear;
    }
}
