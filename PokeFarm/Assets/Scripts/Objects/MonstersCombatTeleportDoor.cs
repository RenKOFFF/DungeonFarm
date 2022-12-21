using System;
using System.Collections;
using System.Collections.Generic;
using Base.Combat;
using UnityEngine;

public class MonstersCombatTeleportDoor : TeleportDoor
{
    private SelectCombatMonsters _selectCombatMonsters;

    private void Start()
    {
        _selectCombatMonsters = GetComponent<SelectCombatMonsters>();
    }

    public override void Interact()
    {
        _selectCombatMonsters.ShowUI();
    }

    public void GoToCombat()
    {
        if (isSwitchingSceneNow) return;
        
        StartCoroutine(WaitSwitchScene());
    }
}
