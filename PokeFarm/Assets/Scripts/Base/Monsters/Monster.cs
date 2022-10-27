using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    public float Speed { get => _speed; private set => _speed = value;  }

    public StateMachine StateMachine;

    [HideInInspector] public RestState RestState;
    [SerializeField] private Transform _restPlace;

    [HideInInspector] public PatrolState RatrolState;
    [SerializeField] private PatrolData PatrolData;

    [HideInInspector] public WanderingState WanderingState;

    private void Start()
    {
        InitializeStates();

        StateMachine = new StateMachine();
        StateMachine.Init(WanderingState);
    }

    private void InitializeStates()
    {
        RestState = new RestState(this, Speed, _restPlace);
        RatrolState = new PatrolState(this, Speed, PatrolData);
        WanderingState = new WanderingState(this, Speed, 5f);
}

    private void Update()
    {
        StateMachine.CurrentState.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (StateMachine.CurrentState.GetType() == RestState.GetType()) StateMachine.ChangeState(RatrolState);
            else StateMachine.ChangeState(RestState);
        }
    }
}
