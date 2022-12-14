using System.Collections;
using System.Collections.Generic;
using Base.Time;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{ 
    public bool isCanInteract => !_isSleepNow;
    private bool _isSleepNow;
    
    public void Interact()
    {
        if (_isSleepNow) return;
        
        StartCoroutine(WaitSleep());
    }

    private IEnumerator WaitSleep()
    {
        _isSleepNow = true;
        
        var waitFading = true;
        Fader.Instance.FadeIn(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }
        
        WorldTimer.Instance.SkipOneDay();
        
        waitFading = true;
        Fader.Instance.FadeOut(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }
        
        _isSleepNow = false;
    }
}
