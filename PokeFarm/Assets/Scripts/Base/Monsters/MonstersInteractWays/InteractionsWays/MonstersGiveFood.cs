using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersGiveFood : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    private void Start()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.SetActive(false);
        }
    }
}
