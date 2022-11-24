using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandStationUI : MonoBehaviour
{
    [field : SerializeField] public GridLayoutGroup CommandList { get; private set; }
    [field: SerializeField] public GridLayoutGroup MonstersList { get; private set; }

    private void Awake()
    {
        //MonstersList.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(HideCommandAndShowMonsters);
    }

    private void HideCommandAndShowMonsters(MonstersInteractionWay _)
    {
        CommandList.gameObject.SetActive(false);
        MonstersList.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(HideCommandAndShowMonsters);
    }
}
