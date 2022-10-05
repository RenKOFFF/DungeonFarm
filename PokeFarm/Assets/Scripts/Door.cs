using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject open, closed;
    protected bool isOpen;
    public virtual void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
        }
        else
        {
            isOpen = false;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
        }
    }
}
