using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    const float FADE_IN_SPEED = 2;

    // set in inspector
    public CanvasFadeGroup fadeGroup;
    [SerializeField] Image outerBar;
    [SerializeField] Image healthBar;
    [SerializeField] Image armorBar;

    Animator animator;

    bool visible = false;

    private void Start()
    {
        fadeGroup.GetComponent<Canvas>().worldCamera = Camera.main;
        animator = GetComponent<Animator>();

        // set colors
        outerBar.color = Color.black;
        healthBar.color = ConfigController.Instance.color.healthBarColor;
        armorBar.color = ConfigController.Instance.color.armorBarColor;
    }

    /// <summary>
    /// set the fill amount of the health bar between 0 and 1
    /// </summary>
    /// <param name="healthFill"></param>
    public void SetFill(float healthFill, float armorFill)
    {
        if (!visible)
        {
            visible = true;

            fadeGroup.Show();
        }

        healthBar.fillAmount = healthFill;
        armorBar.fillAmount = armorFill;
    }
}
