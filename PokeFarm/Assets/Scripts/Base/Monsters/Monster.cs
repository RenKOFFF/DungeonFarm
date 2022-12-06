using UnityEngine;

[RequireComponent(typeof(MonsterBehaviour))]
public class Monster : MonoBehaviour
{
    [field:SerializeField] public MonsterDataSO MonsterData { get; private set; }
    public float Speed { get => _speed; private set => _speed = value;  }
    [SerializeField] private float _speed = 1.5f;
    public float MaxEnergy { get => _maxEnergy; private set => _maxEnergy = value;  }
    [SerializeField] private float _maxEnergy = 8f;
    public ItemStorageContainer Inventory { get; private set; }
    public MonsterBehaviour MonsterBehaviour { get => _monsterBehaviour; }
    private MonsterBehaviour _monsterBehaviour;

    private void Awake()
    {
        //TODO create in MonsterData container size
        Inventory = new ItemStorageContainer(1);
        _monsterBehaviour = GetComponent<MonsterBehaviour>();
    }

    public static Monster Spawn(MonsterDataSO monsterData, Transform parentMonsters)
    {
        var m = monsterData.Prefab;
        m.MonsterData = monsterData;
        m = Instantiate(m, parentMonsters);
        return m;
    }
}
