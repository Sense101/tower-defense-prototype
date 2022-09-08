using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleExtension : MonoBehaviour
{
    [HideInInspector] public Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
}
