using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    public static bool Paused { get; private set; } = false;

    private float _playSpeed = 1;

    public void SetPlaySpeed(float newSpeed)
    {
        _playSpeed = newSpeed;

        if (!Paused)
        {
            Time.timeScale = _playSpeed;
        }
    }

    public float GetPlaySpeed()
    {
        return _playSpeed;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = _playSpeed;
        Paused = false;
    }
}
