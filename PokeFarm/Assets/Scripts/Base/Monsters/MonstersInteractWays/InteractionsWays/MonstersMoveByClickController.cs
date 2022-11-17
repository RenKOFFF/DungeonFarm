using UnityEngine;

public class MonstersMoveByClickController : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    private Camera _cam;
    private Vector2 _mousePos;
    private Vector2 _movePosition;

    public override void Execute()
    {
    }

    public override bool GetDisplayCondition()
    {
        return true;
    }

    private void Start()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        _cam = Camera.main;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _mousePos = Input.mousePosition;
            _movePosition = _cam.ScreenToWorldPoint(_mousePos);

            _monsterBehaviour.StateMachine.ChangeState(new WanderingState(_monsterBehaviour.Monster, _monsterBehaviour.Monster.Speed, 2f, _movePosition));
            gameObject.SetActive(false);
        }
    }
}
