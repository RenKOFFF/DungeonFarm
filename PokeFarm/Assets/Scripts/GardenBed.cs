using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBed : MonoBehaviour, IInteractable
{
    public bool isCanInteract { get => _isCanInteract; private set => _isCanInteract = value; }
    private bool _isCanInteract = true;
    public GardenBedState state { get; private set; }

    [SerializeField] private GameObject empty, planted, growned;

    private void Start()
    {
        state = GardenBedState.Empty;
    }
    public void Interact()
    {
        switch (state)
        {
            case GardenBedState.Empty:
                Debug.Log($"Interacted with {this}");
                _isCanInteract = false;
                empty.SetActive(false);
                planted.SetActive(true);

                state = GardenBedState.Planted;
                break;

            case GardenBedState.Planted:
                Debug.Log("Rastet uydi");
                _isCanInteract = true;
                planted.SetActive(false);
                growned.SetActive(true);

                state = GardenBedState.Growned;
                break;

            case GardenBedState.Growned:
                Debug.Log($"Interacted with {this} to harvest");
                growned.SetActive(false);
                empty.SetActive(true);

                state = GardenBedState.Empty;
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
}
