using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemy : PlayerBase
{
    public override void InitPlayer(string uid)
    {
        base.InitPlayer(uid);
        Type = "Enemy";
        print("Enemy InitPlayer()");
    }

    public override void UpdatePlayer()
    {
        Move();
        Fire();
    }

    public override void Move()
    {

    }

    public override void Fire()
    {

    }
}
