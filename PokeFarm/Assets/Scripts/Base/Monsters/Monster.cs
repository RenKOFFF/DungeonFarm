using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private StateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = new StateMachine();
        _stateMachine.Init(new IdleState());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.ChangeState(new PatrolState());
        }
    }
}
