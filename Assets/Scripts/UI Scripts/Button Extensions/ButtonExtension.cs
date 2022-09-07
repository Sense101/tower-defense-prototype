using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonExtension : MonoBehaviour
{
    [HideInInspector] public Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
