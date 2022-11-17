using UnityEngine;

public abstract class MonstersInteractionWay : MonoBehaviour
{
    [field: SerializeField] public MonstersInteractionWayDataSO MonstersInteractionWayData { get; private set; }

    protected bool _displayCondition = true;
    public abstract bool GetDisplayCondition();

    public abstract void Execute();
}
