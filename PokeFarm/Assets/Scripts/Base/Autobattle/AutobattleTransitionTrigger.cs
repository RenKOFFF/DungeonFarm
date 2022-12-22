using UnityEngine;

public class AutobattleTransitionTrigger : MonoBehaviour
{
    [field: SerializeField] private SelectCombatMonsters SelectCombatMonsters { get; set; }

    public void StartAutobattle()
    {
        GameSceneManager.LoadScene(Scene.Autobattle);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        SelectCombatMonsters.ShowUI();
    }
}
