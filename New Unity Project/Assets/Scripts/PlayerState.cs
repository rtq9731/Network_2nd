using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eState
{
    idle,
    walk,
    trace,
    attack
}

public class PlayerState : MonoBehaviour
{
    public GameObject target;
    public eState state_cur = eState.idle;
    public float state_time ;

    public void State_Start(eState state)
    {
        state_cur = state;
        state_time = Time.time + 1.0f;

        switch (state)
        {
            case eState.idle:
                break;
            case eState.walk:
                break;
            case eState.trace:
                break;
            case eState.attack:
                break;
            default:
                break;
        }
    }                                                                    

    public void state_Update()
    {
        switch (state_cur)
        {
            case eState.idle:
                if (target != null && Vector3.Distance(target.transform.position, transform.position) < 4.0f)
                    State_Start(eState.trace);
                transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                State_Start(eState.walk);
                break;
            case eState.walk:
                if (target != null && Vector3.Distance(target.transform.position, transform.position) < 4.0f)
                    State_Start(eState.trace);
                // move
                transform.position += transform.forward * 0.5f * Time.deltaTime;
                if (Time.time > state_time) State_Start(eState.idle);
                break;
            case eState.trace:
                if (target == null) { State_Start(eState.idle); break; }
                if (Vector3.Distance(target.transform.position, transform.position) > 6.0f)
                {
                    State_Start(eState.idle); break;
                }
                if (Vector3.Distance(target.transform.position, transform.position) < 1.0f)
                {
                    State_Start(eState.attack); break;
                }
                // move trace
                transform.LookAt(target.transform);
                transform.position += transform.forward * 1.0f * Time.deltaTime;
                break;
            case eState.attack:
                if (Vector3.Distance(target.transform.position, transform.position) > 2.0f) { State_Start(eState.trace); break; }
                break;
        }
    }
}


