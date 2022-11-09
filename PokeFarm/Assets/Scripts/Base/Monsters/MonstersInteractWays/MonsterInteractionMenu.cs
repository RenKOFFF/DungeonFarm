using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInteractionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private MonsterInteractionButton[] _interactionButtons;

    private Monster _currentMonster;
    private MonstersInteractionWay[] _monstersInteractionWays;

    private void OnEnable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(Init);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(OnInteracted);
        MonsterInteractionButton.OnInteractedEvent.AddListener(OnInteracted);
    }

    void Start()
    {
        Hide();
    }

    private void Init(Monster currentMonster, List<MonstersInteractionWay> monstersInteractionWays)
    {
        _currentMonster = currentMonster;
        _monstersInteractionWays = monstersInteractionWays.ToArray();

        RefreshInteractButtons();
    }

    private void RefreshInteractButtons()
    {
        _menu.SetActive(true);

        var maxWayIndex = _monstersInteractionWays?.Length - 1;

        for (int i = 0; i < _interactionButtons.Length; i++)
        {
            _interactionButtons[i].gameObject.SetActive(true);
            if (i <= maxWayIndex)
            {
                _interactionButtons[i].InteractData = _monstersInteractionWays[i].MonstersInteractionWayData;
                _interactionButtons[i].InteractionWay = _monstersInteractionWays[i];
                _interactionButtons[i].RefreshButtonData();
            }
            else _interactionButtons[i].gameObject.SetActive(false);
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
        Hide();
        _currentMonster.MonsterBehaviour.MakeNonInteractive(.3f);
    }

    private void OnDisable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(Init);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(OnInteracted);
        MonsterInteractionButton.OnInteractedEvent.RemoveListener(OnInteracted);
    }
}
