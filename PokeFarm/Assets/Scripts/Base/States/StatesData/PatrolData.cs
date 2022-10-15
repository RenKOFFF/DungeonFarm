using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolData : MonoBehaviour
{
    public Transform[] PatrolPoints { get => _patrolPointsList.ToArray();}
    private List<Transform> _patrolPointsList = new List<Transform>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            _patrolPointsList.Add(child);
        }
    }
}
