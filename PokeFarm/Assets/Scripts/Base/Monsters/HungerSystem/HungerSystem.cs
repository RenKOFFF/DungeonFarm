using System;
using Base.Observable;
using Base.Time;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    private readonly ObservableVariable<Satiety> _satietyLevel = new();
    public Satiety SatietyLevel => _satietyLevel.Value;
    public bool IsFedToday { get; private set; }
    private Monster _monster;

    private void Start()
    {
        _monster = GetComponent<Monster>();
    }

    public void Init(Satiety satiety)
    {
        _satietyLevel.Value = satiety;
    }

    public void GetHungry()
    {
        _satietyLevel.Value = 
            _satietyLevel.Value == Satiety.VeryHungry ? 
            Satiety.VeryHungry : 
            _satietyLevel.Value - 1;
        
        IsFedToday = false;
    }
    
    public void FeelFull()
    {
        _satietyLevel.Value = 
            _satietyLevel.Value == Satiety.Full ? 
                Satiety.Full : 
                _satietyLevel.Value + 1;
        
        IsFedToday = true;
    }
    
    
    private void OnEnable()
    {
        WorldTimer.OnDayChangedEvent.AddListener(OnDaySkipped);
    }

    private void OnDisable()
    {
        WorldTimer.OnDayChangedEvent.RemoveListener(OnDaySkipped);
    }

    private void OnDaySkipped()
    {
        if (!IsFedToday) GetHungry();
        IsFedToday = false;
        
        Debug.Log($"{_monster} is {SatietyLevel} \nand {(IsFedToday ? "fed today" : "not fed today")}");
    }
}