using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHero : PlayerBase
{
    public override void InitPlayer(string uid)
    {
        base.InitPlayer(uid);
        Type = "Hero";
        print("Hero InitPlayer()");
    }

    public override void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow)) { transform.position += transform.forward * moveSpeed * Time.deltaTime * 3.0f; }
        if (Input.GetKey(KeyCode.DownArrow)) { transform.position -= transform.forward * moveSpeed * Time.deltaTime * 3.0f; }
        if (Input.GetKey(KeyCode.RightArrow)) { transform.Rotate(new Vector3(0, 90 * Time.deltaTime * 3, 0)); }
        if (Input.GetKey(KeyCode.LeftArrow)) { transform.Rotate(new Vector3(0, -90 * Time.deltaTime * 3, 0)); }
    }
}
