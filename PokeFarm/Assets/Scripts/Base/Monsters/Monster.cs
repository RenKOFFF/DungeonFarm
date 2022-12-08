using Base.Monsters;
using UnityEngine;

[RequireComponent(typeof(MonsterBehaviour))]
public class Monster : MonoBehaviour
{
    [field:SerializeField] public MonsterDataSO MonsterData { get; private set; }
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
        get => _startAttachment;
        private set
        {
            _startAttachment = value;
            MaxEnergy = _startAttachment;
        }
    }

    [SerializeField] private float _startAttachment = 30f;

    public MonsterStats Stats => MonsterData.GetStats();
    
    public MonsterBehaviour MonsterBehaviour { get => _monsterBehaviour; }
    private MonsterBehaviour _monsterBehaviour;

    public ItemStorageContainer Inventory { get; private set; }
    private void Awake()
    {
        _monsterBehaviour = GetComponent<MonsterBehaviour>();
        Inventory = new ItemStorageContainer(Stats.InventorySize);
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
