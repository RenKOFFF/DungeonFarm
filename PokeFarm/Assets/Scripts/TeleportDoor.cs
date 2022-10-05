using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TeleportDoor : Door
{
    public string DurationTeleportBySceneName;
    public override void Interact()
    {
        base.Interact();
        if (isOpen) Teleport();
    }

    private void Teleport()
    {
        SceneManager.LoadScene(DurationTeleportBySceneName);
    }
}
