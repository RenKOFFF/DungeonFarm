using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonstersInteractionWay : MonoBehaviour
{
    [field: SerializeField] public MonstersInteractionWayDataSO MonstersInteractionWayData { get; private set; }
}
