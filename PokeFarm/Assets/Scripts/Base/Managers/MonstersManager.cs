using System;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;

namespace Base.Managers
{
    public class MonstersManager : MonoBehaviour
    {
        public CommandDataSO[] AllMonstersCommand { get; set; }
        public MonsterDataSO[] AllMonstersData;
        public List<Monster> AllMonstersOnTheFarm = new();

        [SerializeField] private Transform _parentMonsters;

        public static MonstersManager Instance;

        private void Awake()
        {
            Instance = this;
            AllMonstersCommand = Resources.LoadAll<CommandDataSO>("Monsters/Commands");
            AllMonstersData = GetAllMonstersData();
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
