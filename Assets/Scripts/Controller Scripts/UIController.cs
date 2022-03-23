using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [HideInInspector] public List<UIElement> _elements = new List<UIElement>();
    private void Start()
    {
        //
    }

    private void Update()
    {

    }
}
