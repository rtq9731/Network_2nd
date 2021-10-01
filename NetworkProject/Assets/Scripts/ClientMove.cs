using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMove : MonoBehaviour
{
    [SerializeField] float speed;

    Vector3 movePos;

    IEnumerator move;

    bool isMove = false;


    public void Move(Vector3 movePos)
    {
        if (move != null)
        {
            StopCoroutine(move);
        }
        else
        {
            move = MoveCorutine(movePos);
        }

        StartCoroutine(move);
    }

    IEnumerator MoveCorutine(Vector3 movePos)
    {
        while(Vector3.Distance(this.transform.position, movePos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, movePos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = movePos;
    }
}
