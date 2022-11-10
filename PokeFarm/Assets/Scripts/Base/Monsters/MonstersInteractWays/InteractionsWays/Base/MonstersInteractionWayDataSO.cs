using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MonstersInteractionWayData", menuName = "Interaction Way/Interaction Way Data")]
public class MonstersInteractionWayDataSO : ScriptableObject
{
    public Sprite Icon { get => _icon; private set => _icon = value; }
    public string InteractName { get => _interactName; private set => _interactName = value; }
    public MonstersInteractionWay InteractionWay { get => _interactionWay; }

    [SerializeField] private Sprite _icon;
    [SerializeField] private string _interactName;
    [SerializeField] private MonstersInteractionWay _interactionWay;
}
