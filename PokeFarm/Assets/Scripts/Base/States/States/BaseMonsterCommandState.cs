using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseMonsterCommandState : BaseMonsterState
{
    protected Vector3Int _currentTilePosition;
    protected TileBase _breakTile;
    protected  int _currentTileIndex = -1;
    
    public override void Update()
    {
        FindNearestTilePosition();
        MoveTo(_currentTilePosition);
    }
    
    private void FindNearestTilePosition()
    {
        if (!LandscapeController.Instance.SpawnableTilesDictionary.ContainsKey(_breakTile))
        {
            //Debug.Log("Not tree for cut down");
            return;
        }
        
        var count = LandscapeController.Instance.SpawnableTilesDictionary[_breakTile].Count;

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
                LandscapeController.Instance.SpawnableTilesDictionary[_breakTile][i],
                monsterPos);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                _currentTileIndex = i;
            }
        }
        _currentTilePosition = LandscapeController.Instance.SpawnableTilesDictionary[_breakTile][_currentTileIndex];
    }
    private void MoveTo(Vector3Int movePoint)
    {
        if (_currentTileIndex == -1 ) return;
        
        if (Vector3.Distance(_monsterBehaviour.Monster.transform.position, movePoint) > .5f)
        {
            _monsterBehaviour.Monster.transform.position = Vector3.MoveTowards
            (_monsterBehaviour.Monster.transform.position, 
                movePoint, 
                Time.deltaTime * _speed);
        }
        else
        {
            //TODO сделать условие удаления - вырубку дерева
            LandscapeController.Instance.SpawnableTilesDictionary[_breakTile].RemoveAt(_currentTileIndex);
            Debug.Log($"Break {_breakTile.name}");
            _currentTileIndex = -1;
            FindNearestTilePosition();
        }
    }
}