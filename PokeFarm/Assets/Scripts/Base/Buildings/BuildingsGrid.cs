using System.Collections.Generic;
using UnityEngine;

namespace Base.Buildings
{
    public class BuildingsGrid : MonoBehaviour
    {
        public static BoundsInt CellBounds { get; private set; }
        public static Buildings[,] GridBuildings { get; private set; }

        [SerializeField] private Transform _parentBuilding;
        [SerializeField] private Buildings _flyingBuilding;
        private bool _isAvailableToBuild;
        private bool _isCanBuild = true;
        
        
        private void Start()
        {
            CellBounds = TileMapReadManager.Instance.backgroundTilemap.cellBounds;
            GridBuildings = new Buildings[CellBounds.size.x, CellBounds.size.y];

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
                    var bPos = building.gameObject.transform.position;
                    WriteBuildData((int)bPos.x, (int)bPos.y, building);
                }
            }
        }

        void Update()
        {
            if (!_isCanBuild) return;
            
            if (ToolbarManager.Instance?.ItemOnTheHand?.type == ItemType.Building)
            {
                _flyingBuilding.gameObject.SetActive(true);
                _flyingBuilding.RefreshItem(ToolbarManager.Instance.ItemOnTheHand);
            
                var mousePosition = TileMapReadManager.Instance.GetCurrentBackgroundGridPositionByMousePosition();
                _flyingBuilding.transform.position = mousePosition;
            
                _isAvailableToBuild = CheckMarker(mousePosition) &&
                               CheckBoundsForBuildAvailable(mousePosition) &&
                               !IsPlaceTaken(mousePosition.x, mousePosition.y);
            
                _flyingBuilding.SetAvailableColor(_isAvailableToBuild);

                if (_isAvailableToBuild && Input.GetMouseButtonDown(0))
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
                    GridBuildings
                    [
                        -CellBounds.xMin + placeX - halfSizeX + x,
                        -CellBounds.yMin + placeY + y
                    ] = building;
                }
            }
        }

        private bool CheckBoundsForBuildAvailable(Vector3Int mousePosition)
        {
            return !(mousePosition.x - _flyingBuilding.Size.x / 2 < CellBounds.xMin ||
                     mousePosition.x + 1  + _flyingBuilding.Size.x / 2 > CellBounds.xMax ||
                     mousePosition.y - _flyingBuilding.Size.y / 2 < CellBounds.yMin ||
                     mousePosition.y + 1  + _flyingBuilding.Size.y / 2 > CellBounds.yMax);
        }

        private bool IsPlaceTaken(int placeX, int placeY)
        {
            var halfSizeX = _flyingBuilding.Size.x / 2;
            var halfSizeY = _flyingBuilding.Size.y / 2;
        
            for (int x = 0; x < _flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < _flyingBuilding.Size.y; y++)
                {
                    if (GridBuildings
                        [
                            placeX - halfSizeX - CellBounds.xMin + x, 
                            placeY - halfSizeY - CellBounds.yMin + y
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
            _isCanBuild = false;
            _flyingBuilding.gameObject.SetActive(false);
            _isAvailableToBuild = false;
        }

        private void ReturnBuild()
        {
            _isCanBuild = true;
            _flyingBuilding.gameObject.SetActive(true);
            _isAvailableToBuild = true;
        }

        private void HideOrReturnBuild(bool isNeedHide)
        {
            _isCanBuild = !isNeedHide;
            _flyingBuilding.gameObject.SetActive(!isNeedHide);
            _isAvailableToBuild = !isNeedHide;
        }
        
        private void OnEnable()
        {
            MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.AddListener(HideBuild);
            MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.AddListener(ReturnBuild);
            MonsterInteractionButton.OnInteractedEvent.AddListener(ReturnBuild);
            CommandStationUI.IsUiActiveEvent.AddListener(HideOrReturnBuild);
            InventoryManager.IsInventoryOpenEvent.AddListener(HideOrReturnBuild);
        }

        private void OnDisable()
        {
            MonsterBehaviour.OnPlayerCalledInteractionMenuEvent.RemoveListener(HideBuild);
            MonsterBehaviour.OnPlayerExitInteractionDistanceEvent.RemoveListener(ReturnBuild);
            MonsterInteractionButton.OnInteractedEvent.RemoveListener(ReturnBuild);
            CommandStationUI.IsUiActiveEvent.RemoveListener(HideOrReturnBuild);
            InventoryManager.IsInventoryOpenEvent.RemoveListener(HideOrReturnBuild);
        }
    }
}
