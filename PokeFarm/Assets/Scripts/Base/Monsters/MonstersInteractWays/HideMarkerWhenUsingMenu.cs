using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMarkerWhenUsingMenu : MonoBehaviour
{
    [SerializeField] private MarkerManager _gridMarker;

    private void OnEnable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(HideMarker);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(ReturnMarker);
        MonsterInteractionButton.OnInteractedEvent.AddListener(ReturnMarker);
    }

    private void HideMarker(Monster _, List<MonstersInteractionWay> __)
    {
        _gridMarker?.gameObject.SetActive(false);
    }

    private void ReturnMarker()
    {
        _gridMarker?.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(HideMarker);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(ReturnMarker);
        MonsterInteractionButton.OnInteractedEvent.RemoveListener(ReturnMarker);
    }

}
