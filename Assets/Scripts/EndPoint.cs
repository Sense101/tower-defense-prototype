using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : PathPoint
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        // do nothing
    }

    public override void OnEnemyArrive(Enemy enemy)
    {
        //@TODO fully implement this
        Destroy(enemy.gameObject);
    }
}
