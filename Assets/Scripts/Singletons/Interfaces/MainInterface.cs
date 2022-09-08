using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainInterface : Singleton<MainInterface>
{
    //set in inspector
    public UIToggleSelector hotbarSelector;
    public TextMeshProUGUI coinText;
    public CanvasFadeGroup overlayFilter;
}
