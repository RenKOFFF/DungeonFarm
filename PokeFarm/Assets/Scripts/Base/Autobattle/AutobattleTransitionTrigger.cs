using System.Collections;
using UnityEngine;

public class AutobattleTransitionTrigger : MonoBehaviour
{
    [field: SerializeField] private SelectCombatMonsters SelectCombatMonsters { get; set; }

    public void StartAutobattle()
    {
        StartCoroutine(WaitSwitchScene());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        SelectCombatMonsters.ShowUI();
    }

    private IEnumerator WaitSwitchScene()
    {
        var waitFading = true;
        Fader.Instance.FadeIn(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }

        GameSceneManager.LoadScene(Scene.Autobattle);

        waitFading = true;
        Fader.Instance.FadeOut(() => waitFading = false);
        while (waitFading)
        {
            yield return null;
        }
    }
}
