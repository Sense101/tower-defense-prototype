using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class GunInfo
{
    // all set in inspector
    public Sprite sprite; // does sprite even need to be stored if animator is changing it? @TODO
    public Vector2 localPosition;
    public float localRotation;
    public RuntimeAnimatorController animatorController;
}
