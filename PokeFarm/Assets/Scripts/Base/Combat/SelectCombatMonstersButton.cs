using System;
using Base.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.Combat
{
    public class SelectCombatMonstersButton : MonoBehaviour
    {
        private Button _button;
        [HideInInspector] public Monster AttachedMonster;
        
        private Image _icon;
        private TextMeshProUGUI _name;
        private bool _isInited;

        public static UnityEvent<Monster> OnSelectMonsterEvent = new();

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        private void Start()
        {
            Init();
            RefreshData();
            //_button.interactable = false;
        }

        private void Init()
        {
            _icon = GetComponentInChildren<Image>();
            _isInited = true;
        }
        
        public void RefreshData()
        {
            if (!_isInited) Init();
            if (!AttachedMonster)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);

            _icon.sprite = AttachedMonster.MonsterData.Icon;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Interact);
        
        }
        public void Interact()
        {
            MonstersManager.Instance.SelectCombatMonster(AttachedMonster);
            
            OnSelectMonsterEvent.Invoke(AttachedMonster);
            
            AttachedMonster = null;
            RefreshData();
            
        }
        private void OnDisable()
        {
            _button.onClick.RemoveListener(Interact);
        }
    }
}