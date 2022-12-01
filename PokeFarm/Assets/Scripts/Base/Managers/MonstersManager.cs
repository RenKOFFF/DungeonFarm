using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Managers
{
    public class MonstersManager : MonoBehaviour
    {
        public static MonstersManager Instance;

        public List<Monster> AllMonstersOnTheFarm = new();
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
                var monster = Monster.Spawn(monsterDataSo);
                AllMonstersOnTheFarm.Add(monster);
                return monster;
            }
        }
    }
}