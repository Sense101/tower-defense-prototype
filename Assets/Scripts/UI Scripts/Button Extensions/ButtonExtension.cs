using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIButton))]
public class ButtonExtension : MonoBehaviour
{
    [HideInInspector] public UIButton button;

    private void Awake()
    {
        button = GetComponent<UIButton>();
    }
}
