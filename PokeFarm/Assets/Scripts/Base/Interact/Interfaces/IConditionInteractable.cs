using UnityEngine.Events;

public interface IConditionInteractable : IInteractable
{
    public UnityEvent<IInteractable> OnConditionUpdatedEvent { get; }
    bool Condition { get; }
}

