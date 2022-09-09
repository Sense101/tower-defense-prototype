using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIToggle))]
public class ToggleExtension : MonoBehaviour
{
    [HideInInspector] public UIToggle toggle;

    private void Awake()
    {
        toggle = GetComponent<UIToggle>();
    }
}
