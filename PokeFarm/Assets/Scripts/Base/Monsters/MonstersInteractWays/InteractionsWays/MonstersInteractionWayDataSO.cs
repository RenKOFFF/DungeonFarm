using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonstersInteractionWayData", menuName = "InteractionWay/MonstersInteractionWayData")]
public class MonstersInteractionWayDataSO : ScriptableObject
{
    [SerializeField] Sprite _icon;
    public Sprite Icon { get => _icon; private set => _icon = value; }

    [SerializeField] public string _name;
    public string Name { get => _name; private set => _name = value; }
}
