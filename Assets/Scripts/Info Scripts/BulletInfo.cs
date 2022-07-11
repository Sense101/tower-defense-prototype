using UnityEngine;

// contains all the info needed to set up a bullet
[System.Serializable]
public class BulletInfo
{
    public Sprite sprite;
    public RuntimeAnimatorController controller;
    public Bullet.Type type;
}
