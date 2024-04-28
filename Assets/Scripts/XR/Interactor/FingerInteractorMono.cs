using System;
using UnityEngine;
using Unity.Entities;

[Serializable]
public struct FingerTransformPair {
    [HideInInspector]
    public string name;
    public Entity e;
    public Transform Base;
    public Transform Tip;
}

[Serializable]
public struct FingerBezier {
    [HideInInspector]
    public string name;
    public GameObject Instance;
    public BezierLine BezierLine;
    public GameObject Interactable;
}

public class FingerInteractorMono : MonoBehaviour
{
    private string[] fingerNames = new string[5] {"Thumb", "Index", "Middle", "Ring", "Finger"};
    [SerializeField] private GameObject FingerHolderPrefab;
    [SerializeField] private FingerTransformPair[] fingers = new FingerTransformPair[5];

    [SerializeField] private GameObject FingerBezierPrefab;
    [SerializeField] private FingerBezier[] FingerBeziers = new FingerBezier[5];

    private EntityManager entityManager;

    void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        for (int i = 0; i < 5; i++) {
            fingers[i].e = entityManager.CreateEntity(typeof(FingerReference), typeof(FingerData), typeof(FingerInteractableReference));
            fingers[i].name = fingerNames[i];

            var inst = Instantiate(FingerBezierPrefab);
            FingerBeziers[i] = new FingerBezier{
                name = fingerNames[i],
                Instance = inst,
                BezierLine = inst.GetComponent<BezierLine>(),
            };

            entityManager.SetComponentData(fingers[i].e, new FingerReference{
                Base = fingers[i].Base, Tip = fingers[i].Tip, ThumbTip = fingers[0].Tip,
            });

            //entityManager.SetComponentData(fingers[i].e, new LineReference{Line = inst.GetComponent<BezierLine>()});
        }

        UpdateFingerVisibility(false);
    }

    public void UpdateFingerVisibility(bool b)
    {
        for (int i = 0; i < 5; i++) {
            entityManager.SetComponentEnabled<FingerData>(fingers[i].e, b);
        }
    }

    void Start()
    {
        for (int i = 0; i < 5; i++) {
            FingerBeziers[i].BezierLine.Visibility = false;
        }
    }

    void Update()
    {
        for(int i = 0; i < 5; i++) {
            if (FingerBeziers[i].Interactable == null) continue;

            
        }
    }

    public void OnEnter(GameObject obj) {
        for (int i = 0; i < 5; i++) {
            entityManager.SetComponentData(fingers[i].e, new FingerInteractableReference{
                InteractableObject = obj
            });
        }
    }

    public void OnExit(GameObject obj) {
        for (int i = 0; i < 5; i++) {
            //if (entityManager.GetComponentData<FingerInteractableReference>(fingers[i].e).Object != obj) continue;
            entityManager.SetComponentData(fingers[i].e, new FingerInteractableReference{
                InteractableObject = null
            });
        }
    }

}