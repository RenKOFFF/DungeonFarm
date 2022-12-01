using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Managers
{
    public class MonstersManager : MonoBehaviour
    {
        public List<Monster> AllMonstersOnTheFarm = new();
        [SerializeField] private Transform _parentMonsters; 
        
        public static MonstersManager Instance;
        
        private void Awake() => Instance = this;
        
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