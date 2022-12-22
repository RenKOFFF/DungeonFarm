using System;
using System.Linq;
using Base.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Combat
{
    public class SelectCombatMonstersUI : MonoBehaviour
    {
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private GameObject _selectButtonParent;
        private SelectCombatMonstersButton[] _sButtons;
        [SerializeField] private GameObject _deselectButtonParent;
        private DeselectCombatMonstersButton[] _dsButtons;

        private void Start()
        {
            InitSelectCmb();
            InitDeselectCmb();
            gameObject.SetActive(false);
        }

        private void InitSelectCmb()
        {
            var selectCmbCount = MonstersManager.Instance.AllMonstersOnTheFarm.Count + 
                                 MonstersManager.Instance.SelectedCombatMonsters.Count;
            _sButtons = new SelectCombatMonstersButton[selectCmbCount];

            for (int i = 0; i < selectCmbCount; i++)
            {
                if (!_sButtons[i])
                {
                    _sButtons[i] = Instantiate(_buttonPrefab, _selectButtonParent.transform).
                        AddComponent<SelectCombatMonstersButton>();
                }

                if (i < MonstersManager.Instance.AllMonstersOnTheFarm.Count)
                {
                    _sButtons[i].AttachedMonster = MonstersManager.Instance.AllMonstersOnTheFarm[i];
                }
                _sButtons[i].RefreshData();
            }
        }
        
        private void InitDeselectCmb()
        {
            var deselectCmbCount = MonstersManager.Instance.AllMonstersOnTheFarm.Count + 
                                   MonstersManager.Instance.SelectedCombatMonsters.Count;
            _dsButtons = new DeselectCombatMonstersButton[deselectCmbCount];

            for (int i = 0; i < deselectCmbCount; i++)
            {
                if (!_dsButtons[i])
                {
                    _dsButtons[i] = Instantiate(_buttonPrefab, _deselectButtonParent.transform).
                        AddComponent<DeselectCombatMonstersButton>();
                }
                
                if (i < MonstersManager.Instance.SelectedCombatMonsters.Count)
                {
                    _dsButtons[i].AttachedMonster = MonstersManager.Instance.SelectedCombatMonsters[i];
                }
                _dsButtons[i].RefreshData();
            }
        }

        private void RefreshDsButtons(Monster monster)
        {
            var b = _dsButtons.
                ToList().
                Find(b => b.AttachedMonster == null);
            
            b.AttachedMonster = monster;
            b.RefreshData();
        }
        
        private void RefreshSButtons(Monster monster)
        {
            var b = _sButtons.
                ToList().
                Find(b => b.AttachedMonster == null);
            
            b.AttachedMonster = monster;
            b.RefreshData();
        }

        private void OnEnable()
        {
            SelectCombatMonstersButton.OnSelectMonsterEvent.AddListener(RefreshDsButtons);
            DeselectCombatMonstersButton.OnDeselectMonsterEvent.AddListener(RefreshSButtons);
        }

        private void OnDisable()
        {
            SelectCombatMonstersButton.OnSelectMonsterEvent.RemoveListener(RefreshDsButtons);
            DeselectCombatMonstersButton.OnDeselectMonsterEvent.RemoveListener(RefreshSButtons);
        }
    }
}