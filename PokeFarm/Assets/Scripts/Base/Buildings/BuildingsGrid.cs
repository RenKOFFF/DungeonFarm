using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    private Vector2Int _gridSize;
    private Buildings[,] _grid;

    [SerializeField] private Transform _parentBuilding;
    [SerializeField] private Buildings _flyingBuilding;

    private void OnEnable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(HideBuild);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(ReturnBuild);
        MonsterInteractionButton.OnInteractedEvent.AddListener(ReturnBuild);
    }

    private void Start()
    {
        var cellBounds = TileMapReadManager.Instance.backgroundTilemap.cellBounds;
        var size = cellBounds.max - cellBounds.min;
        _grid = new Buildings[size.x, size.y];
    }

    void Update()
    {
        if (HandBank.instance?.itemOnTheHand?.type == ItemType.Building)
        {
            _flyingBuilding?.gameObject.SetActive(true);
            _flyingBuilding.transform.position = TileMapReadManager.Instance.GetCurrentBackgroundGridPositionByMousePosition();
            _flyingBuilding.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, 128);
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(_flyingBuilding, _parentBuilding);
            }
        }
        else
        {
            _flyingBuilding?.gameObject.SetActive(false);
        }
    }

    private void HideBuild(Monster _, List<MonstersInteractionWay> __)
    {
        gameObject.SetActive(false);
    }

    private void ReturnBuild()
    {
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(HideBuild);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(ReturnBuild);
        MonsterInteractionButton.OnInteractedEvent.RemoveListener(ReturnBuild);
    }
}
