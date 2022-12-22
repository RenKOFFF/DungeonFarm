using System;
using System.Collections;
using System.Collections.Generic;
using Base.Combat;
using UnityEngine;

public class SelectCombatMonsters : MonoBehaviour
{
    [SerializeField] private SelectCombatMonstersUI _ui;

    private void Start()
    {
        HideUI();
    }

    public void ShowUI()
    {
        _ui.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        _ui.gameObject.SetActive(false);
    }
}
