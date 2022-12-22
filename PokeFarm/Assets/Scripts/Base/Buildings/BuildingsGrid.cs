using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    private BoundsInt _cellBounds;
    private Buildings[,] _gridBuildings;

    [SerializeField] private Transform _parentBuilding;
    [SerializeField] private Buildings _flyingBuilding;
    private bool _isAvailable;

    private void OnEnable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(HideBuild);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(ReturnBuild);
        MonsterInteractionButton.OnInteractedEvent.AddListener(ReturnBuild);
    }

    private void Start()
    {
        _cellBounds = TileMapReadManager.Instance.backgroundTilemap.cellBounds;
        _gridBuildings = new Buildings[_cellBounds.size.x, _cellBounds.size.y];

        var startBuildings = GameObject.FindGameObjectsWithTag("Building");
        InitStartBuildings(startBuildings);
    }

    private void InitStartBuildings(GameObject[] startBuildings)
    {
        if (startBuildings == null) return;
        
        foreach (var b in startBuildings)
        {
            var building = b.GetComponent<Buildings>();
            if (building)
            {
                var bPos = building.transform.position;
                WriteBuildData((int)bPos.x, (int)bPos.y, building);
            }
        }
    }

    void Update()
    {
        if (ToolbarManager.Instance?.ItemOnTheHand?.type == ItemType.Building)
        {
            _flyingBuilding.gameObject.SetActive(true);
            _flyingBuilding.RefreshItem(ToolbarManager.Instance.ItemOnTheHand);
            
            var mousePosition = TileMapReadManager.Instance.GetCurrentBackgroundGridPositionByMousePosition();
            _flyingBuilding.transform.position = mousePosition;
            
            _isAvailable = CheckMarker(mousePosition) &&
                           CheckBoundsForBuildAvailable(mousePosition) &&
                           !IsPlaceTaken(mousePosition.x, mousePosition.y);
            
            _flyingBuilding.SetTransparentColor(_isAvailable);

            if (_isAvailable && Input.GetMouseButtonDown(0))
            {
                DoBuild(mousePosition.x, mousePosition.y);
            }
        }
        else
        {
            _flyingBuilding?.gameObject.SetActive(false);
        }
    }

    private bool CheckMarker(Vector3Int mousePosition)
    {
        return mousePosition == MarkerManager.Instance.markedCellPosition;
    }

    private void DoBuild(int placeX, int placeY)
    {
        _flyingBuilding.SetDefaultColor();

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

        WriteBuildData(placeX, placeY, _flyingBuilding);

        GameManager.Instance.inventory.Remove(ToolbarManager.Instance.ItemOnTheHand);
    }

    private void WriteBuildData(int placeX, int placeY, Buildings building)
    {
        var halfSizeX = building.Size.x / 2;
        var halfSizeY = building.Size.y / 2;

        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                _gridBuildings
                [
                    placeX - halfSizeX - _cellBounds.xMin + x,
                    placeY - halfSizeY - _cellBounds.yMin + y
                ] = building;
            }
        }
    }

    private bool CheckBoundsForBuildAvailable(Vector3Int mousePosition)
    {
        return !(mousePosition.x - _flyingBuilding.Size.x / 2 < _cellBounds.xMin ||
                mousePosition.x + 1  + _flyingBuilding.Size.x / 2 > _cellBounds.xMax ||
                mousePosition.y - _flyingBuilding.Size.y / 2 < _cellBounds.yMin ||
                mousePosition.y + 1  + _flyingBuilding.Size.y / 2 > _cellBounds.yMax);
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        var halfSizeX = _flyingBuilding.Size.x / 2;
        var halfSizeY = _flyingBuilding.Size.y / 2;
        
        for (int x = 0; x < _flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < _flyingBuilding.Size.y; y++)
            {
                if (_gridBuildings
                    [
                        placeX - halfSizeX - _cellBounds.xMin + x, 
                        placeY - halfSizeY - _cellBounds.yMin + y
                    ] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void HideBuild(Monster _, List<MonstersInteractionWay> __)
    {
        _isAvailable = false;
        gameObject.SetActive(false);
    }

    private void ReturnBuild()
    {
        gameObject.SetActive(true);
        _isAvailable = true;
    }

    private void OnDisable()
    {
        MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(HideBuild);
        MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(ReturnBuild);
        MonsterInteractionButton.OnInteractedEvent.RemoveListener(ReturnBuild);
    }
}
