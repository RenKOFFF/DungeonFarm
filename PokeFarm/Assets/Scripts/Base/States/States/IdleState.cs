using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        Debug.Log("I'm idle now");
    }

    public override void Exit()
    {
        Debug.Log("I'm not idle now");
    }
}
