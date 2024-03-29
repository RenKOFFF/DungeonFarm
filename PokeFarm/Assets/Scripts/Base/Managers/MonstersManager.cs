﻿using System;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;

namespace Base.Managers
{
    public class MonstersManager : MonoBehaviour
    {
        public CommandDataSO[] AllMonstersCommand { get; set; }
        private MonsterDataSO[] _allMonstersData;
        public List<Monster> AllMonstersOnTheFarm = new();
        public static List<Monster> SelectedCombatMonsters = new();

        [SerializeField] private Transform _parentMonsters;

        public static MonstersManager Instance;

        private void Awake()
        {
            Instance = this;
            SelectedCombatMonsters = new List<Monster>();
            _allMonstersData = GetAllMonstersData();
            AllMonstersCommand = Resources.LoadAll<CommandDataSO>("Monsters/Commands");
        }

        public bool SelectCombatMonster(Monster monster)
        {
            var combatMonster = AllMonstersOnTheFarm.Find(m => m == monster);
            if (combatMonster)
            {
                AllMonstersOnTheFarm.Remove(combatMonster);
                SelectedCombatMonsters.Add(combatMonster);
                
                return true;
            }

            return false;
        }

        public bool DeselectCombatMonster(Monster monster)
        {
            var combatMonster = SelectedCombatMonsters.Find(m => m == monster);
            if (combatMonster)
            {
                SelectedCombatMonsters.Remove(combatMonster);
                AllMonstersOnTheFarm.Add(combatMonster);
                
                return true;
            }

            return false;
        }

        public static MonsterDataSO[] GetAllMonstersData()
            => Resources.LoadAll<MonsterDataSO>("Monsters");

        public Monster GetMonsterInstance(MonsterDataSO monsterDataSo)
        {
            var m = AllMonstersOnTheFarm.Find(m => m.MonsterData == monsterDataSo);
            if (m)
            {
                return m;
            }
            else
            {
                var monster = Monster.Spawn(monsterDataSo, _parentMonsters);
                AllMonstersOnTheFarm.Add(monster);
                return monster;
            }
        }
    }
}
