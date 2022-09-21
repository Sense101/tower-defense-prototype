using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : ScaleButton
{
    public override void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
