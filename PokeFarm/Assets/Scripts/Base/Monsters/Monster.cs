using UnityEngine;

[RequireComponent(typeof(MonsterBehaviour))]
public class Monster : MonoBehaviour
{
    [field:SerializeField] public MonsterDataSO MonsterData { get; private set; }
    
    public float Speed { get => _speed; private set => _speed = value;  }
    [SerializeField] private float _speed = 1.5f;
    
    public float MaxEnergy
    {
        get => _maxEnergy;
        private set
        {
            CurrentEnergy /= _maxEnergy;
            _maxEnergy = value;
            CurrentEnergy *= _maxEnergy;
        }
    }
    private float _maxEnergy = 30;

    public float CurrentEnergy { get; private set; }

    public float Attachment
    {
        get => _attachment;
        private set
        {
            _attachment = value;
            MaxEnergy = _attachment;
        }
    }

    [SerializeField] private float _attachment = 30f;

    public MonsterBehaviour MonsterBehaviour { get => _monsterBehaviour; }
    private MonsterBehaviour _monsterBehaviour;

    public ItemStorageContainer Inventory { get; private set; }
    private void Awake()
    {
        //TODO create in MonsterData container size
        Inventory = new ItemStorageContainer(1);
        _monsterBehaviour = GetComponent<MonsterBehaviour>();
        CurrentEnergy = MaxEnergy;
    }
    
    public bool SpendEnergy(int value = 1)
    {
        if (CurrentEnergy - value >= 0)
        {
            CurrentEnergy -= value;
            return true;
        }
        
        return false;
    }

    public void RestoreEnergy(int value = 1)
    {
        if (CurrentEnergy + value <= MaxEnergy)
        {
            CurrentEnergy+= value;
        }

        CurrentEnergy = MaxEnergy;
    }
    
    public bool SpendAttachment(int value = 1)
    {
        if (Attachment - value >= 0)
        {
            Attachment -= value;
            return true;
        }
        
        return false;
    }

    public void AddAttachment(int value = 1)
    {
        if (Attachment + value <= 100)
        {
            Attachment += value;
        }

        CurrentEnergy = 100;
    }

    public static Monster Spawn(MonsterDataSO monsterData, Transform parentMonsters)
    {
        //TODO Спавнить заранее через это
        var m = monsterData.Prefab;
        m.MonsterData = monsterData;
        m = Instantiate(m, parentMonsters);
        return m;
    }
}
