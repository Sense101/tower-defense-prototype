using System.Runtime;
using UnityEngine;

// this is the script that will handle all the interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    public Turret selectedTurret = null;

    // set in inspector
    [Space(5)]
    public UIButtonSelector targetTypeSelector;

    private void Start()
    {

    }

    private void Update()
    {

    }



    /////////// bunch of scripts called by the buttons within the interface
}
