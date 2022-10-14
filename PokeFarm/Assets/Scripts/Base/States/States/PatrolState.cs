using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public override void Enter()
    {
        Debug.Log("I'm patrol now");
    }

    public override void Exit()
    {
        Debug.Log("I'm not patrol now");
    }
}
