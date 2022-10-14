using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public interface IConditionInteractable : IInteractable
{
    public UnityEvent<IInteractable> OnConditionUpdatedEvent { get; }
    bool Condition { get; }
}

