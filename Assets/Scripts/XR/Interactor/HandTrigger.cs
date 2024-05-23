using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    [SerializeField] private FingerInteractorMono fingerInteractor;

    
    void OnTriggerEnter(Collider other)
    {
        fingerInteractor.OnEnter(other.gameObject);
        Debug.Log("other entered: " + other.gameObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        fingerInteractor.OnExit(other.gameObject);
        Debug.Log("other exited: " + other.gameObject.name);
    }

}
