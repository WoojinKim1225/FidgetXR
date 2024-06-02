using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{
    [SerializeField] private FingerInteractorMono fingerInteractor;

    
    void OnTriggerEnter(Collider other)
    {
        if (fingerInteractor == null) return;
        fingerInteractor.OnEnter(other.gameObject);
        Debug.Log("other entered: " + other.gameObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (fingerInteractor == null) return;
        fingerInteractor.OnExit(other.gameObject);
        Debug.Log("other exited: " + other.gameObject.name);
    }

}
