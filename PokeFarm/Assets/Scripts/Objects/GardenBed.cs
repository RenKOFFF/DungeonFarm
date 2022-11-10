using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GardenBed : MonoBehaviour, IConditionInteractable
{
    public Item seed;
    public float plantGrowingTime = 7f; 

    public bool isCanInteract { get => _isCanInteract; private set => _isCanInteract = value; }
    private bool _isCanInteract = true;

    public bool Condition { get => _condition; private set => _condition = value; }
    private bool _condition;

    public GardenBedState state { get; private set; }

    public UnityEvent<IInteractable> OnConditionUpdatedEvent => _onConditionUpdatedEvent;
    private UnityEvent<IInteractable> _onConditionUpdatedEvent = new UnityEvent<IInteractable>();

    [SerializeField] private GameObject empty, planted, growned;

    private void Start()
    {
        state = GardenBedState.Empty;
        //UpdateCondition();
        ToolbarController.Instance.OnItemOnTheHandChanged.AddListener(UpdateCondition);
    }

    private void UpdateCondition()
    {
        if (state != GardenBedState.Empty) return;

        Condition = ToolbarController.Instance.ItemOnTheHand == seed;
        OnConditionUpdatedEvent.Invoke(this);
    }

    public void Interact()
    {
        switch (state)
        {
            case GardenBedState.Empty:
                _isCanInteract = false;

                empty.SetActive(false);
                planted.SetActive(true);

                StartCoroutine(Growing());

                Condition = true;
                state = GardenBedState.Planted;
                break;

            case GardenBedState.Growned:
                growned.SetActive(false);
                empty.SetActive(true);

                state = GardenBedState.Empty;
                UpdateCondition();
                break;

            default:
                Debug.Log("WTF!");
                break;
        }
    }

    public enum GardenBedState
    {
        Empty,
        Planted,
        Growned
    }

    IEnumerator Growing()
    {
        yield return new WaitForSeconds(plantGrowingTime);

        Debug.Log("Growned");
        _isCanInteract = true;
        planted.SetActive(false);
        growned.SetActive(true);

        state = GardenBedState.Growned;

        yield break;
    }
}
