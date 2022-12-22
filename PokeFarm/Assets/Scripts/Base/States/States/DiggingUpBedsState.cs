using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.States.States
{
    public class DiggingUpBedsState : BaseMonsterState
    {
        protected Vector3Int _currentTilePosition;
        protected PlantingCycleTile _diggingUpTile;
        protected  int _currentTileIndex = -1;
        protected Item _workTool;
        public int CommandEnergyCost { get; protected set; }
        
        private Dictionary<PlantingCycleTile, List<Vector3Int>> _plantingTrackingDictionary = new ();

        public DiggingUpBedsState(Monster monster, PlantingCycleTile diggingUpPlantingCycleTile, Item workTool, int commandEnergyCost)
        {
            _monsterBehaviour = monster.MonsterBehaviour;
            _speed = monster.Stats.Speed;

            _waitTime = _startWaitTime;
            
            _diggingUpTile = diggingUpPlantingCycleTile;
            _workTool = workTool;
            CommandEnergyCost = commandEnergyCost;
        }

        public override void Enter()
        {
            base.Enter();
            CharacterMouseInteractController.OnPlantHarvestedEvent.AddListener(AddPlantTracking);
            ToolsController.OnGardenBedDigUpEvent.AddListener(AddPlantTracking);
            
            FindNearestTilePosition();
        }
        
        private void AddPlantTracking(PlantingCycleTile plantingCycleTile, Vector3Int position)
        {
            if (plantingCycleTile == null)
            {
                var key = _plantingTrackingDictionary.Keys.ToArray()[0];
                _plantingTrackingDictionary[key].Add(position);
            }
            else
                _plantingTrackingDictionary[plantingCycleTile].Add(position);
        }

        public override void Exit()
        {
            base.Exit();
            ToolsController.OnGardenBedDigUpEvent.RemoveListener(AddPlantTracking);
            CharacterMouseInteractController.OnPlantHarvestedEvent.RemoveListener(AddPlantTracking);
            _plantingTrackingDictionary.Clear();
        }

        public override void Update()
        {
            MoveTo(_currentTilePosition);
        }
    
        private void FindNearestTilePosition()
        {
            if (_plantingTrackingDictionary.Keys.Count <= 0)
            {
                //Debug.Log("Not tree for cut down");
                return;
            }
            
            var count = _plantingTrackingDictionary[_diggingUpTile].Count;

            if (count == 0)
            {
                //Debug.Log("Not tree for cut down");
                return;
            }
            
            var monsterPos = _monsterBehaviour.Monster.gameObject.transform.position;
            var minDistance = float.MaxValue;
            
            for (int i = 0; i < count; i++)
            {
                var distance = Vector3.Distance(
                    _plantingTrackingDictionary[_diggingUpTile][i],
                    monsterPos);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    _currentTileIndex = i;
                }
            }
            _currentTilePosition = _plantingTrackingDictionary[_diggingUpTile][_currentTileIndex];
        }
        private void MoveTo(Vector3Int movePoint)
        {
            if (_currentTileIndex == -1 ) return;
            
            if (Vector3.Distance(_monsterBehaviour.Monster.transform.position, movePoint) > .5f)
            {
                _monsterBehaviour.Monster.transform.position = Vector3.MoveTowards
                (_monsterBehaviour.Monster.transform.position, 
                    movePoint, 
                    UnityEngine.Time.deltaTime * _speed);
            }
            else
            {
                if (IsWorkDone())
                {
                    //_plantingCTrackingDictionary[_diggingUpTile].RemoveAt(_currentTileIndex);
                    Debug.Log($"Dig up {_diggingUpTile.name}");
                    _currentTileIndex = -1;

                    MonsterToolController.SwitchCycleTileUsingTool(_workTool, _monsterBehaviour.Monster, _diggingUpTile, _currentTilePosition);

                    if (!_monsterBehaviour.Monster.SpendEnergy(CommandEnergyCost))
                    {
                        _monsterBehaviour.StateMachine.ChangeState(_monsterBehaviour.StateMachine.DefaultState);
                    }
                    
                    FindNearestTilePosition();   
                }
            }
        }

        private bool IsWorkDone()
        {
            _waitTime -= UnityEngine.Time.deltaTime;
            if (_waitTime <= 0)
            {
                _waitTime = _startWaitTime;
                return true;
            }
            return false;
        }
    }
}