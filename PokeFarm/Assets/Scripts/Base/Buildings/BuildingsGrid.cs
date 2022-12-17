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
        if (ToolbarManager.Instance?.ItemOnTheHand?.type == ItemType.Building)
        {
            _flyingBuilding.gameObject.SetActive(true);
            
            _flyingBuilding.RefreshItem(ToolbarManager.Instance.ItemOnTheHand);
            _flyingBuilding.transform.position = TileMapReadManager.Instance.GetCurrentBackgroundGridPositionByMousePosition();
            _flyingBuilding.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, 128);
            if (Input.GetMouseButtonDown(0))
            {
                if (ToolbarManager.Instance.ItemOnTheHand.BuildingPrefab)
                {
                    Instantiate(ToolbarManager.Instance.ItemOnTheHand.BuildingPrefab,
                        _flyingBuilding.transform.position,
                        Quaternion.identity,
                        _parentBuilding);
                }
                else
                {
                    Instantiate(_flyingBuilding, _parentBuilding);
                }
                
                GameManager.Instance.inventory.Remove(ToolbarManager.Instance.ItemOnTheHand);
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
