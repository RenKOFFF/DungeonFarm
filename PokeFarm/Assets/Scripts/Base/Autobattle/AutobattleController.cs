using Base.Managers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldCell
{
    public Vector2 Position { get; set; }
    [CanBeNull] public MonsterDataSO MonsterData { get; set; }
    [CanBeNull] public GameObject SpawnedMonster { get; set; }
}

public class AutobattleController : MonoBehaviour
{
    [field: SerializeField] private Tilemap BackgroundTilemap { get; set; }
    [field: SerializeField] private GameObject MonsterPrefab { get; set; }
    [field: SerializeField] private float TimeToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] private Vector3Int GridOffset { get; set; } = new(-3, -3);
    [field: SerializeField] private int CellDistanceBetweenFields { get; set; } = 3;

    private const int FieldWidth = 2;
    private const int FieldHeight = 3;

    private float TimeLeftToMakeMoveInSeconds { get; set; }
    private bool IsPlayerTurn { get; set; } = true;

    private FieldCell[,] PlayerField { get; set; }
    private FieldCell[,] EnemyField { get; set; }

    private void Initialize()
    {
        PlayerField = new FieldCell[FieldWidth, FieldHeight];
        EnemyField = new FieldCell[FieldWidth, FieldHeight];

        for (var w = 0; w < FieldWidth; w++)
        {
            for (var h = 0; h < FieldHeight; h++)
            {
                PlayerField[w, h] = new FieldCell();
                EnemyField[w, h] = new FieldCell();
            }
        }

        var availableMonsters = MonstersManager.GetAllMonstersData();

        PlayerField[0, 0].MonsterData = availableMonsters[0];
        PlayerField[1, 1].MonsterData = availableMonsters[1];
        PlayerField[0, 2].MonsterData = availableMonsters[1];

        for (var w = 0; w < FieldWidth; w++)
        {
            for (var h = 0; h < FieldHeight; h++)
            {
                var fieldCell = PlayerField[w, h];
                if (fieldCell.MonsterData != null)
                {
                    fieldCell.Position = TileMapReadManager.GetCellCenterWorldPosition(
                        BackgroundTilemap,
                        new Vector3Int(w, h) + GridOffset);

                    fieldCell.SpawnedMonster = SpawnManager.SpawnObject(fieldCell.Position, MonsterPrefab);
                    fieldCell.SpawnedMonster.transform.parent = transform;
                    fieldCell.SpawnedMonster.name = $"Monster [{w},{h}]";
                }
            }
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        TimeLeftToMakeMoveInSeconds -= Time.deltaTime;

        if (TimeLeftToMakeMoveInSeconds < 0)
        {
            MakeMove(IsPlayerTurn ? PlayerField : EnemyField);

            IsPlayerTurn = !IsPlayerTurn;
            TimeLeftToMakeMoveInSeconds = TimeToMakeMoveInSeconds;
        }
    }

    private void MakeMove(FieldCell[,] field) { }
}
