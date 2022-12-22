using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : Door
{
    protected bool isSwitchingSceneNow;
    public string durationTeleportBySceneName;

    public override void Interact()
    {
        if (isSwitchingSceneNow) return;
        
        StartCoroutine(WaitSwitchScene());
    }

    protected void Teleport()
    {
        SceneManager.LoadScene(durationTeleportBySceneName);
    }
    
    protected IEnumerator WaitSwitchScene()
    {
        isSwitchingSceneNow = true;
        
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
        
        isSwitchingSceneNow = false;
    }
}
