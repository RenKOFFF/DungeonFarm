using System;
using Base.Managers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

internal class FieldCell
{
    public Vector2 Position { get; set; }
    [CanBeNull] public MonsterDataSO MonsterData { get; set; }
    [CanBeNull] public AutobattleMonster SpawnedMonster { get; set; }
}

public class AutobattleController : MonoBehaviour
{
    [field: SerializeField] private Tilemap BackgroundTilemap { get; set; }
    [field: SerializeField] private AutobattleMonster MonsterPrefab { get; set; }
    [field: SerializeField] private Vector3Int GridOffset { get; set; } = new(-3, -1);
    [field: SerializeField] private int CellDistanceBetweenFields { get; set; } = 3;

    private const int FieldWidth = 2;
    private const int FieldHeight = 3;

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

        EnemyField[0, 0].MonsterData = availableMonsters[0];
        EnemyField[1, 1].MonsterData = availableMonsters[1];
        EnemyField[0, 2].MonsterData = availableMonsters[1];

        for (var w = 0; w < FieldWidth; w++)
        {
            for (var h = 0; h < FieldHeight; h++)
            {
                SpawnMonster(PlayerField, w, h);
                SpawnMonster(EnemyField, w, h, true);
            }
        }
    }

    private void SpawnMonster(FieldCell[,] field, int w, int h, bool isSecondField = false)
    {
        var cell = field[w, h];

        if (cell.MonsterData != null)
        {
            var secondFieldOffset = new Vector3Int(isSecondField ? CellDistanceBetweenFields + FieldWidth : 0, 0);

            cell.Position = TileMapReadManager.GetCellCenterWorldPosition(
                BackgroundTilemap,
                new Vector3Int(w, h) + GridOffset + secondFieldOffset);

            cell.SpawnedMonster = SpawnManager.SpawnObject(cell.Position, MonsterPrefab);
            var spawnedMonster = cell.SpawnedMonster;

            spawnedMonster.transform.parent = transform;
            spawnedMonster.name = $"Monster [{w},{h}]";
            spawnedMonster.Initialize(cell.MonsterData.GetStats());
        }
    }

    private void ForEachFieldCell(Action<FieldCell> action)
    {
        for (var w = 0; w < FieldWidth; w++)
        for (var h = 0; h < FieldHeight; h++)
        {
            action.Invoke(PlayerField[w, h]);
            action.Invoke(EnemyField[w, h]);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        PlayerField.AttackOnField(EnemyField);
        EnemyField.AttackOnField(PlayerField);

        ForEachFieldCell(cell =>
        {
            if (cell.SpawnedMonster is { IsDead: true })
            {
                cell.SpawnedMonster.Die();
                cell.SpawnedMonster = null;
            }
        });
    }
}

internal static class FieldExtensions
{
    internal static void ForEach(this FieldCell[,] field, Action<FieldCell> action)
    {
        for (var w = 0; w < field.GetLength(0); w++)
        for (var h = 0; h < field.GetLength(1); h++)
            action.Invoke(field[w, h]);
    }

    internal static void AttackOnField(this FieldCell[,] attackerField, FieldCell[,] defenderField)
    {
        attackerField.ForEach(playerCell =>
        {
            var attackingMonster = playerCell.SpawnedMonster;

            if (attackingMonster == null)
                return;

            attackingMonster.TimeLeftToMakeMoveInSeconds -= Time.deltaTime;

            if (attackingMonster.TimeLeftToMakeMoveInSeconds < 0)
            {
                defenderField.ForEach(enemyCell =>
                {
                    if (enemyCell.SpawnedMonster != null && !enemyCell.SpawnedMonster.IsDead)
                        attackingMonster.Attack(enemyCell.SpawnedMonster);
                });

                attackingMonster.TimeLeftToMakeMoveInSeconds = attackingMonster.TimeToMakeMoveInSeconds;
            }
        });
    }
}
