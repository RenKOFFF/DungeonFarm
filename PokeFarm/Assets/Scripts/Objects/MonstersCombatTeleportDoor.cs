using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersCombatTeleportDoor : TeleportDoor
{
    private bool _isSwitchingSceneNow;

    public override void Interact()
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
