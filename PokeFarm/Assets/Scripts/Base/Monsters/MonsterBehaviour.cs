using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Monster))]
public class MonsterBehaviour : MonoBehaviour, IInteractable
{
    private Monster _monster;
    public Monster Monster { get => _monster; private set => _monster = value; }
    public float Speed { get => _monster.Stats.Speed;}
    public bool isCanInteract => _isCanInteract;
    private bool _isCanInteract = true;

    [Header("States")]
    public StateMachine StateMachine = new();
    
    [HideInInspector] public RestState RestState;
    //[SerializeField] private Transform _restPlace;

    [HideInInspector] public PatrolState RatrolState;
    //[SerializeField] private PatrolData PatrolData;

    [HideInInspector] public WanderingState WanderingState;
    [HideInInspector] public FollowingState FollowingState;

    [SerializeField] private GameObject _monstersInteractionWaysParent;
    private List<MonstersInteractionWay> _monstersInteractionWays = new List<MonstersInteractionWay>();
    private MonsterInteractionMenu _interactionMenu;
    public static UnityEvent<Monster, List<MonstersInteractionWay>> OnPlayerCalledInteractionMenuEvent = new UnityEvent<Monster, List<MonstersInteractionWay>>();
    public static UnityEvent OnPlayerExitInteractionDistanceEvent = new UnityEvent();

    private void Awake()
    {
        _interactionMenu = GameObject.FindGameObjectWithTag("MonstersInteractionMenu").GetComponent<MonsterInteractionMenu>();
        _monstersInteractionWays = GetComponentsInChildren<MonstersInteractionWay>().ToList();
    }
    private void Start()
    {
        _monster = GetComponent<Monster>();

        InitializeInteractionWays();
        InitializeStates();
        
        StateMachine.Init(WanderingState);
    }

    private void InitializeInteractionWays()
    {
        var interactionWaysDataSO = _monster?.MonsterData?.InteractionWay;
        foreach (var interactionWayDataSO in interactionWaysDataSO)
        {
            var interactionWay = Instantiate(interactionWayDataSO.InteractionWay, _monstersInteractionWaysParent.transform);
            _monstersInteractionWays.Add(interactionWay);
        }
    }

    private void InitializeStates()
    {
        //RestState = new RestState(_monster, _monster.Speed, _restPlace);
        //RatrolState = new PatrolState(_monster, _monster.Speed, PatrolData);
        WanderingState = new WanderingState(_monster, _monster.Stats.Speed, 5f);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }

    public bool GiveCommand(BaseMonsterCommandState commandState)
    {
        if (Monster.SpendAttachment())
        {
            StateMachine.ChangeState(commandState);
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collider = collision.GetComponent<CharacterController2D>();
        if (collider != null)
        {
            StateMachine.ChangeState(new FollowingState(_monster, _monster.Stats.Speed, collider.gameObject, StateMachine.CurrentState));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var collider = collision.GetComponent<CharacterController2D>();
        if (collider != null)
        {
            OnPlayerExitInteractionDistanceEvent.Invoke();
        }
    }

    public void Interact()
    {
        OnPlayerCalledInteractionMenuEvent.Invoke(_monster, _monstersInteractionWays);
        StateMachine.ChangeState(new WaitState(Monster, Speed, StateMachine.CurrentState));
        Debug.Log($"Im interact with {_monster.name} - вызвана менюшка");
    }

    public void MakeNonInteractive(float time)
    {
        _isCanInteract = false;
        Invoke(nameof(MakeInteractive), time);
    }

    private void MakeInteractive()
    {
        _isCanInteract = true;
    }
}
