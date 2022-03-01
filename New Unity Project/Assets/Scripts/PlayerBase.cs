using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public GameObject GO;
    public string UID;
    public string NickName { get; set; }
    public string Type { get; set; }

    public float moveSpeed = 6;
    public float spinSpeed = 180f;

    GameObject target;

    public virtual void InitPlayer(string uid)
    {
        UID = uid;
        GO = gameObject;
        Type = "Base";
        print("Base InitPlayer()");
    }
    public virtual void UpdatePlayer()
    {
        Move();
        Fire();
    }

    public virtual void Fire()
    {
        //throw new NotImplementedException();
    }

    public virtual void Move()
    {
        //throw new NotImplementedException();
    }
}
