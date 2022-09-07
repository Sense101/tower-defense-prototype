using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInterface : Singleton<MainInterface>
{
    //@todo this is not nice
    //set in inspector
    public Hotbar hotbar;
    public CanvasFadeGroup overlayFilter;
}
