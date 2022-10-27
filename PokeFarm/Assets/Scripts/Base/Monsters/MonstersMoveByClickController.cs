using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersMoveByClickController : MonoBehaviour
{
    private Monster _monster;

    private Camera _cam;
    private Vector2 _mousePos;
    private Vector2 _movePosition;

    private void Start()
    {
        _monster = GetComponent<Monster>();
        _cam = Camera.main;

        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _mousePos = Input.mousePosition;
            _movePosition = _cam.ScreenToWorldPoint(_mousePos);

            _monster.StateMachine.ChangeState(new WanderingState(_monster, _monster.Speed, 2f, _movePosition));
            enabled = false;
        }
    }
}
