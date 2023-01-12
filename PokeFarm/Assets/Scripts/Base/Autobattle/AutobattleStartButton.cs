using Base.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Autobattle
{
    public class AutobattleStartButton : MonoBehaviour
    {
        private Button ButtonComponent { get; set; }

        private void Awake()
        {
            ButtonComponent = GetComponent<Button>();
        }

        private void Update()
        {
            ButtonComponent.interactable = MonstersManager.SelectedCombatMonsters.Count > 0;
        }
    }
}
