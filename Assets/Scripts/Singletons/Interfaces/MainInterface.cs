using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainInterface : Singleton<MainInterface>
{
    //set in inspector
    public UIToggleSelector hotbarSelector;
    public CanvasFadeGroup overlayFilter;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI healthText;
}
