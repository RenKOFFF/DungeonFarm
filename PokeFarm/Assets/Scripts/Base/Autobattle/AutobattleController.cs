using System;
using System.Collections.Generic;
using System.Linq;
using Base.Managers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

internal class FieldCell
{
    [CanBeNull] public MonsterDataSO MonsterData { get; set; }
    [CanBeNull] public AutobattleMonster SpawnedMonster { get; set; }

    public int X { get; }
    public int Y { get; }
    public bool IsPlayerCell { get; }

    public FieldCell(int x, int y, bool isPlayerCell)
    {
        X = x;
        Y = y;
        IsPlayerCell = isPlayerCell;
    }

    public void KillMonsterInCell()
    {
        SpawnedMonster?.Die();
        SpawnedMonster = null;
    }
}

public class AutobattleController : MonoBehaviour
{
    [field: SerializeField] private Vector3Int GridOffset { get; set; } = new(-3, -1);
    [field: SerializeField] private int CellDistanceBetweenFields { get; set; } = 3;
    [field: SerializeField] private Tilemap BackgroundTilemap { get; set; }
    [field: SerializeField] private AutobattleMonster MonsterPrefab { get; set; }
    [field: SerializeField] private TMP_Text EndGameText { get; set; }

    private static Random Random { get; } = new();

    private const int FieldWidth = 2;
    private const int FieldHeight = 3;

    private bool IsGameStarted { get; set; }

    private FieldCell[,] PlayerField { get; set; }
    private FieldCell[,] EnemyField { get; set; }
    private FieldCell LastFieldCell { get; set; }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    private void Initialize(IList<MonsterDataSO> playerMonsters)
    {
        InitializeFields();

        var enemyMonsters = GetEnemyMonsters();

        if (playerMonsters.Count > 0)
            SetRandomMonstersLocation(PlayerField, playerMonsters);

        SetRandomMonstersLocation(EnemyField, enemyMonsters);

        SpawnAllMonsters();
    }

    private void InitializeFields()
    {
        PlayerField = new FieldCell[FieldWidth, FieldHeight];
        EnemyField = new FieldCell[FieldWidth, FieldHeight];

        for (var x = 0; x < FieldWidth; x++)
        {
            for (var y = 0; y < FieldHeight; y++)
            {
                PlayerField[x, y] = new FieldCell(x, y, true);
                EnemyField[x, y] = new FieldCell(x, y, false);
            }
        }
    }

    private static List<MonsterDataSO> GetEnemyMonsters()
    {
        var allMonsters = MonstersManager.GetAllMonstersData();
        var enemyMonsters = new List<MonsterDataSO>();

        for (var i = 0; i < Math.Min(2, allMonsters.Length); i++)
        {
            var randomIndex = Random.Next(0, allMonsters.Length);
            enemyMonsters.Add(allMonsters[randomIndex]);
        }

        return enemyMonsters;
    }

    private static void SetRandomMonstersLocation(FieldCell[,] field, IList<MonsterDataSO> monstersPool)
    {
        while (monstersPool.Count > 0)
        {
            var randomWidth = Random.Next(0, FieldWidth);
            var randomHeight = Random.Next(0, FieldHeight);

            var existingMonsterData = field[randomWidth, randomHeight].MonsterData;

            if (existingMonsterData != null)
                continue;

            var monster = monstersPool[0];
            field[randomWidth, randomHeight].MonsterData = monster;
            monstersPool.Remove(monster);
        }
    }

    private void SpawnAllMonsters()
    {
        for (var x = 0; x < FieldWidth; x++)
        for (var y = 0; y < FieldHeight; y++)
        {
            SpawnMonster(PlayerField, x, y);
            SpawnMonster(EnemyField, x, y, isSecondField: true);
        }
    }

    private void SpawnMonster(FieldCell[,] field, int x, int y, bool isSecondField = false)
    {
        var cell = field[x, y];
        var cellMonsterData = cell.MonsterData;

        if (cellMonsterData == null)
            return;

        var secondFieldOffset = new Vector3Int(isSecondField ? CellDistanceBetweenFields + FieldWidth : 0, 0);

        var position = TileMapReadManager.GetCellCenterWorldPosition(
            BackgroundTilemap,
            new Vector3Int(x, FieldHeight - 1 - y) + GridOffset + secondFieldOffset);

        var spawnedMonster = SpawnManager.SpawnObject(position, MonsterPrefab);
        cell.SpawnedMonster = spawnedMonster;

        var spriteRenderer = spawnedMonster.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cellMonsterData.Icon;

        if (isSecondField)
            spriteRenderer.flipX = !spriteRenderer.flipX;

        spawnedMonster.transform.parent = transform;
        spawnedMonster.name = $"Monster [{x},{y}]";
        var monsterStats = cellMonsterData.GetStats();

#if DEBUG
        monsterStats.Strength = 10;
        monsterStats.Health = 50;

        if (isSecondField)
        {
            monsterStats.Strength -= 5;
            monsterStats.Health -= 5;
        }
#endif

        spawnedMonster.Initialize(monsterStats);
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
        var playerMonsters = MonstersManager.SelectedCombatMonsters
            .Select(m => m.MonsterData)
            .ToList();

        Initialize(playerMonsters);
    }

    private void Update()
    {
        if (!IsGameStarted || LastFieldCell?.SpawnedMonster is { IsPlayingAnimation: true })
            return;

        //TODO: refactor this

        if (LastFieldCell == null || LastFieldCell.IsPlayerCell)
        {
            for (var attackerY = LastFieldCell?.Y ?? 0; attackerY < PlayerField.GetLength(1); attackerY++)
            for (var attackerX = LastFieldCell?.X ?? 0; attackerX < PlayerField.GetLength(0); attackerX++)
            {
                var attackerCell = PlayerField[attackerX, attackerY];
                var attackingMonster = attackerCell.SpawnedMonster;

                if (attackingMonster == null)
                    continue;

                if (attackingMonster.JustPlayedAnimation)
                {
                    attackingMonster.JustPlayedAnimation = false;
                    LastFieldCell = null;
                    continue;
                }

                attackingMonster.ReduceTimeLeftToMakeMove(Time.deltaTime);

                if (!attackingMonster.IsReadyToAttack)
                    continue;

                for (var defenderY = 0; defenderY < EnemyField.GetLength(1); defenderY++)
                for (var defenderX = 0; defenderX < EnemyField.GetLength(0); defenderX++)
                {
                    var defenderCell = EnemyField[defenderX, defenderY];

                    if (defenderCell.SpawnedMonster is not { IsDead: false })
                        continue;

                    attackingMonster.Attack(defenderCell.SpawnedMonster);

                    if (defenderCell.SpawnedMonster.IsDead)
                        defenderCell.KillMonsterInCell();

                    LastFieldCell = attackerCell;
                    return;
                }

                if (attackingMonster.IsPlayingAnimation)
                    return;
            }
        }

        for (var attackerY = LastFieldCell?.Y ?? 0; attackerY < EnemyField.GetLength(1); attackerY++)
        for (var attackerX = LastFieldCell?.X ?? 0; attackerX < EnemyField.GetLength(0); attackerX++)
        {
            var attackerCell = EnemyField[attackerX, attackerY];
            var attackingMonster = attackerCell.SpawnedMonster;

            if (attackingMonster == null)
                continue;

            if (attackingMonster.JustPlayedAnimation)
            {
                attackingMonster.JustPlayedAnimation = false;
                LastFieldCell = null;
                continue;
            }

            attackingMonster.ReduceTimeLeftToMakeMove(Time.deltaTime);

            if (!attackingMonster.IsReadyToAttack)
                continue;

            for (var defenderY = 0; defenderY < PlayerField.GetLength(1); defenderY++)
            for (var defenderX = PlayerField.GetLength(0) - 1; defenderX >= 0; defenderX--)
            {
                var defenderCell = PlayerField[defenderX, defenderY];

                if (defenderCell.SpawnedMonster is not { IsDead: false })
                    continue;

                attackingMonster.Attack(defenderCell.SpawnedMonster);

                if (defenderCell.SpawnedMonster.IsDead)
                    defenderCell.KillMonsterInCell();

                LastFieldCell = attackerCell;
                return;
            }

            if (attackingMonster.IsPlayingAnimation)
                return;
        }

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

        ChangeEndGameText(isPlayerWon);
        EndGameText.transform.parent.gameObject.SetActive(true);
    }

    private void ChangeEndGameText(bool isPlayerWon)
    {
        switch (isPlayerWon)
        {
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

            void EnemyCellAction(FieldCell enemyCell)
            {
                if (!attackingMonster.IsReadyToAttack)
                    return;

                if (enemyCell.SpawnedMonster is not { IsDead: false })
                    return;

                attackingMonster.Attack(enemyCell.SpawnedMonster);

                if (enemyCell.SpawnedMonster.IsDead)
                    enemyCell.KillMonsterInCell();
            }

            if (reverseDefenderFieldByXAxis)
            {
                defenderField.ForEachWithXReversed(EnemyCellAction);
                return;
            }

            defenderField.ForEach(EnemyCellAction);
        });
    }
}
