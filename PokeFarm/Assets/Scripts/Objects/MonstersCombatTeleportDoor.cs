using System;
using System.Collections;
using System.Collections.Generic;
using Base.Combat;
using UnityEngine;

public class MonstersCombatTeleportDoor : TeleportDoor
{
    private bool _isSwitchingSceneNow;
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
        if (_isSwitchingSceneNow) return;
        
        StartCoroutine(WaitSwitchScene());
    }
    
    private IEnumerator WaitSwitchScene()
    {
        _isSwitchingSceneNow = true;
        
        var waitFading = true;
        Fader.Instance.FadeIn(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }
        
        Teleport();
        
        waitFading = true;
        Fader.Instance.FadeOut(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }
        
        _isSwitchingSceneNow = false;
    }
}
