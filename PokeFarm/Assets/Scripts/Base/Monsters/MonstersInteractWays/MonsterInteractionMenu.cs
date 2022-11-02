using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteractionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        _menu.SetActive(true);
    }

    public void Hide()
    {
        _menu.SetActive(false);
    }
}
