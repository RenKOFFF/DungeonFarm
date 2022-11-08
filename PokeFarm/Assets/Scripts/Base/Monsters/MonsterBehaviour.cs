using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Monster))]
public class MonsterBehaviour : MonoBehaviour, IInteractable
{
    private Monster _monster;
    public Monster Monster { get => _monster; private set => _monster = value; }
    public float MonsterSpeed { get => _monster.Speed;}
    public bool isCanInteract => _isCanInteract;
    private bool _isCanInteract;

    [Header("States")]
    public StateMachine StateMachine;

    [HideInInspector] public RestState RestState;
    //[SerializeField] private Transform _restPlace;

    [HideInInspector] public PatrolState RatrolState;
    //[SerializeField] private PatrolData PatrolData;

    [HideInInspector] public WanderingState WanderingState;
    [HideInInspector] public FollowingState FollowingState;

    [SerializeField] private GameObject _monstersInteractionWaysParent;
    private MonstersInteractionWay[] _monstersInteractionWays;
    private MonsterInteractionMenu _interactionMenu;
    public static UnityEvent<Monster, MonstersInteractionWay[]> OnPlayerCalledInteractionMenuEvent = new UnityEvent<Monster, MonstersInteractionWay[]>();

    private void Awake()
    {
        _interactionMenu = GameObject.FindGameObjectWithTag("MonstersInteractionMenu").GetComponent<MonsterInteractionMenu>();
        _monstersInteractionWays = GetComponentsInChildren<MonstersInteractionWay>();
    }
    private void Start()
    {
        _monster = GetComponent<Monster>();

        InitializeStates();

        StateMachine = new StateMachine();
        StateMachine.Init(WanderingState);
    }

    private void InitializeStates()
    {
        //RestState = new RestState(_monster, _monster.Speed, _restPlace);
        //RatrolState = new PatrolState(_monster, _monster.Speed, PatrolData);
        WanderingState = new WanderingState(_monster, _monster.Speed, 5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collider = collision.GetComponent<CharacterController2D>();
        if (collider != null)
        {
            _isCanInteract = true;
            StateMachine.ChangeState(new FollowingState(_monster, _monster.Speed, collider.gameObject));
        }
    }

    public void Interact()
    {
        OnPlayerCalledInteractionMenuEvent.Invoke(_monster, _monstersInteractionWays);
        Debug.Log($"Im interact with {_monster.name} - вызвана менюшка");
    }
}
