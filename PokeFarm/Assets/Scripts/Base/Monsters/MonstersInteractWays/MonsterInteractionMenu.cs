using System.Collections.Generic;
using UnityEngine;

public class MonsterInteractionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private RectTransform _interactionCircle;
    [SerializeField] private MonsterInteractionButton[] _interactionButtons;

    private Monster _currentMonster;
    private MonstersInteractionWay[] _monstersInteractionWays;

    void Start()
    {
        Hide();
        ToolbarManager.Instance.OnItemOnTheHandChanged.AddListener(RefreshInteractButtons);
    }

    private void Update()
    {
        if (_currentMonster) _interactionCircle.position = _currentMonster.transform.position;
    }

    private void Init(Monster currentMonster, List<MonstersInteractionWay> monstersInteractionWays)
    {
        _currentMonster = currentMonster;
        _monstersInteractionWays = monstersInteractionWays.ToArray();

        Show();
    }

    private void RefreshInteractButtons()
    {
        var maxWayIndex = _monstersInteractionWays?.Length - 1;

        for (int i = 0; i < _interactionButtons.Length; i++)
        {
            _interactionButtons[i].gameObject.SetActive(true);
            _interactionButtons[i].Button.interactable = true;
            if (i <= maxWayIndex)
            {
                _interactionButtons[i].InteractData = _monstersInteractionWays[i].MonstersInteractionWayData;
                _interactionButtons[i].InteractionWay = _monstersInteractionWays[i];
                _interactionButtons[i].RefreshButtonData();

                if (!_monstersInteractionWays[i].GetDisplayCondition())
                {
                    //_interactionButtons[i].gameObject.SetActive(false);
                    _interactionButtons[i].Button.interactable = false;
                }
            }
            else
            {
                _interactionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        _menu.SetActive(true);
        RefreshInteractButtons();
    }

    public void Hide()
    {
        _menu.SetActive(false);
    }
    private void OnInteracted()
    {
        _currentMonster?.MonsterBehaviour.MakeNonInteractive(.3f);
        Hide();
    }

    private void OnEnable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(Init);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(OnInteracted);
        MonsterInteractionButton.OnInteractedEvent.AddListener(OnInteracted);
    }
    
    private void OnDisable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(Init);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(OnInteracted);
        MonsterInteractionButton.OnInteractedEvent.RemoveListener(OnInteracted);
    }
}
