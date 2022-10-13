using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public static class ConditionalInteracteItem
{
    public static IConditionInteractable Item { get; private set; }
    public static bool IsInitialized { get; private set; }

    public static UnityEvent OnConditionalInteracteItemInitializedEvent = new UnityEvent();

    public static void Initialize(IConditionInteractable item)
    {
        Item = item;
        IsInitialized = true;
        OnConditionalInteracteItemInitializedEvent.Invoke();
    }
}
