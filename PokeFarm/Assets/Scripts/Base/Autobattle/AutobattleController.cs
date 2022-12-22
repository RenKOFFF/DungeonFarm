using System;
using System.Collections.Generic;
using Base.Managers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

internal class FieldCell
{
    [CanBeNull] public MonsterDataSO MonsterData { get; set; }
    [CanBeNull] public AutobattleMonster SpawnedMonster { get; set; }
}

public class AutobattleController : MonoBehaviour
{
    [field: SerializeField] private Vector3Int GridOffset { get; set; } = new(-3, -1);
    [field: SerializeField] private int CellDistanceBetweenFields { get; set; } = 3;
    [field: SerializeField] private Tilemap BackgroundTilemap { get; set; }
    [field: SerializeField] private AutobattleMonster MonsterPrefab { get; set; }
    [field: SerializeField] private TMP_Text EndGameText { get; set; }

    private const int FieldWidth = 2;
    private const int FieldHeight = 3;

    private FieldCell[,] PlayerField { get; set; }
    private FieldCell[,] EnemyField { get; set; }

    private void Initialize(IList<MonsterDataSO> availableMonsters)
    {
        PlayerField = new FieldCell[FieldWidth, FieldHeight];
        EnemyField = new FieldCell[FieldWidth, FieldHeight];

        for (var x = 0; x < FieldWidth; x++)
        {
            for (var y = 0; y < FieldHeight; y++)
            {
                PlayerField[x, y] = new FieldCell();
                EnemyField[x, y] = new FieldCell();
            }
        }

        // TODO: Убрать, заменить на случайную генерацию
        PlayerField[0, 0].MonsterData = availableMonsters[0];
        PlayerField[1, 1].MonsterData = availableMonsters[1];
        PlayerField[0, 2].MonsterData = availableMonsters[1];

        EnemyField[0, 0].MonsterData = availableMonsters[0];
        EnemyField[1, 1].MonsterData = availableMonsters[1];
        EnemyField[0, 2].MonsterData = availableMonsters[1];

        for (var x = 0; x < FieldWidth; x++)
        {
            for (var y = 0; y < FieldHeight; y++)
            {
                SpawnMonster(PlayerField, x, y);
                SpawnMonster(EnemyField, x, y, true);
            }
        }
    }

    private void SpawnMonster(FieldCell[,] field, int x, int y, bool isSecondField = false)
    {
        var cell = field[x, y];

        if (cell.MonsterData != null)
        {
            var secondFieldOffset = new Vector3Int(isSecondField ? CellDistanceBetweenFields + FieldWidth : 0, 0);

            var position = TileMapReadManager.GetCellCenterWorldPosition(
                BackgroundTilemap,
                new Vector3Int(x, FieldHeight - 1 - y) + GridOffset + secondFieldOffset);

            cell.SpawnedMonster = SpawnManager.SpawnObject(position, MonsterPrefab);
            var spawnedMonster = cell.SpawnedMonster;

            if (isSecondField)
            {
                var spriteRenderer = spawnedMonster.GetComponent<SpriteRenderer>();
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            spawnedMonster.transform.parent = transform;
            spawnedMonster.name = $"Monster [{x},{y}]";
            spawnedMonster.Initialize(cell.MonsterData.GetStats());
        }
    }

    private void ForEachFieldCell(Action<FieldCell> action)
    {
        for (var x = 0; x < FieldWidth; x++)
        for (var y = 0; y < FieldHeight; y++)
        {
            action.Invoke(PlayerField[x, y]);
            action.Invoke(EnemyField[x, y]);
        }
    }

    private void Start()
    {
        Initialize(MonstersManager.GetAllMonstersData());
    }

    private void Update()
    {
        PlayerField.AttackOnField(EnemyField);
        EnemyField.AttackOnField(PlayerField, reverseDefenderFieldByXAxis: true);

        ForEachFieldCell(cell =>
        {
            if (cell.SpawnedMonster is { IsDead: true })
            {
                cell.SpawnedMonster.Die();
                cell.SpawnedMonster = null;
            }
        });

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        var isPlayerWon = true;
        var isEnemyWon = true;

        PlayerField.ForEach(cell =>
        {
            if (cell.SpawnedMonster is { IsDead: false })
                isEnemyWon = false;
        });

        EnemyField.ForEach(cell =>
        {
            if (cell.SpawnedMonster is { IsDead: false })
                isPlayerWon = false;
        });

        if (!isPlayerWon && !isEnemyWon)
            return;

        ChangeEndGameText(isPlayerWon, isEnemyWon);
        EndGameText.transform.parent.gameObject.SetActive(true);
    }

    private void ChangeEndGameText(bool isPlayerWon, bool isEnemyWon)
    {
        switch (isPlayerWon)
        {
            case true when isEnemyWon:
                EndGameText.text = "Ничья";
                EndGameText.color = Color.white;
                return;
            case true:
                EndGameText.text = "Победа";
                EndGameText.color = Color.green;
                return;
            default:
                EndGameText.text = "Поражение";
                EndGameText.color = Color.red;
                break;
        }
    }
}

internal static class FieldExtensions
{
    internal static void ForEach(this FieldCell[,] field, Action<FieldCell> action)
    {
        for (var x = 0; x < field.GetLength(0); x++)
        for (var y = 0; y < field.GetLength(1); y++)
            action.Invoke(field[x, y]);
    }

    internal static void ForEachWithXReversed(this FieldCell[,] field, Action<FieldCell> action)
    {
        for (var x = field.GetLength(0) - 1; x >= 0; x--)
        for (var y = 0; y < field.GetLength(1); y++)
            action.Invoke(field[x, y]);
    }

    internal static void AttackOnField(
        this FieldCell[,] attackerField,
        FieldCell[,] defenderField,
        bool reverseDefenderFieldByXAxis = false)
    {
        attackerField.ForEach(playerCell =>
        {
            var attackingMonster = playerCell.SpawnedMonster;

            if (attackingMonster == null)
                return;

            attackingMonster.ReduceTimeLeftToMakeMove(Time.deltaTime);

            if (!attackingMonster.IsReadyToAttack)
                return;

            void DefenderAction(FieldCell enemyCell)
            {
                if (!attackingMonster.IsReadyToAttack)
                    return;

                if (enemyCell.SpawnedMonster != null && !enemyCell.SpawnedMonster.IsDead)
                    attackingMonster.Attack(enemyCell.SpawnedMonster);
            }

            if (reverseDefenderFieldByXAxis)
            {
                defenderField.ForEachWithXReversed(DefenderAction);
                return;
            }

            defenderField.ForEach(DefenderAction);
        });
    }
}
