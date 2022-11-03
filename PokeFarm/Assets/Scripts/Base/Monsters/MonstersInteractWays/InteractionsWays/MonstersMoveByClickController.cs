using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersMoveByClickController : MonstersInteractionWay
{
    private Monster _monster;

    private Camera _cam;
    private Vector2 _mousePos;
    private Vector2 _movePosition;

    private void Start()
    {
        _monster = GetComponentInParent<Monster>();
        _cam = Camera.main;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _mousePos = Input.mousePosition;
            _movePosition = _cam.ScreenToWorldPoint(_mousePos);

            _monster.StateMachine.ChangeState(new WanderingState(_monster, _monster.Speed, 2f, _movePosition));
            gameObject.SetActive(false);
        }
    }
}
