using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private GameObject heartMesh;
    [SerializeField] 
    private int heal = 50;
    
    private PropShowingBehavior propShowingBehavior;

    [Header("Recovery")]
    [SerializeField]
    private float recoveryTime = 20f;

    private float timerRecovery;
    private bool heartTaken = false;

    void Start()
    {
        propShowingBehavior = GetComponent<PropShowingBehavior>();
        propShowingBehavior.HasProp(true);
    }

    void Update()
    {
        if (heartTaken)
        {
            timerRecovery += Time.deltaTime;
            if (timerRecovery >= recoveryTime)
            {
                heartTaken = false;
                heartMesh.SetActive(true);
                timerRecovery = 0;
                propShowingBehavior.HasProp(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out var target) && other.GetComponent<CharacterController>() != null)
        {
            target.AddHealth(heal);
            heartMesh.SetActive(false);
            heartTaken = true;
            propShowingBehavior.HasProp(false);
        }
    }
}
