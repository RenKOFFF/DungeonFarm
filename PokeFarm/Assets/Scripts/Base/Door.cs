using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject open, closed;
    private bool isOpen;
    public void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
            Debug.Log("Pi, Door is Opened!"); 
        }
        else
        {
            isOpen = false;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
            Debug.Log("Pi, Door is Closed!");
        }
    }
}
