using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : BaseMonsterState
{
    private bool _isCurrentStateDuringInteractionMenu;
    public State PreviousState { get; private set; }

    public WaitState(Monster monster, float speed)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
    }

    public WaitState(Monster monster, float speed, State previousState) : this(monster, speed)
    {
        PreviousState = previousState;
        _isCurrentStateDuringInteractionMenu = true;
    }

    private void ChangeStateOnPrevious()
    {
        _monsterBehaviour.StateMachine.ChangeState(PreviousState);
    }

    public override void Enter()
    {
        if (_isCurrentStateDuringInteractionMenu)
        {
            MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(ChangeStateOnPrevious);
        }
        Debug.Log($"Enter {this}");
        //TODO add animator, maybe, ya chto znayu kak ono budet rabotat'
    }


    public override void Exit()
    {
        if (_isCurrentStateDuringInteractionMenu)
        {
            MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(ChangeStateOnPrevious);
        }
        Debug.Log($"Exit {this}");
    }
}
