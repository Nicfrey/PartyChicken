using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Crown : MonoBehaviour
{
    [SerializeField] 
    private Light lightCrown;
    [SerializeField]
    private Transform visual;
    private PlayerCrownHandling currentOwner;
    public PlayerCrownHandling CurrentOwner => currentOwner;
    private PropShowingBehavior propShowingBehavior;

    private void Start()
    {
        propShowingBehavior = GetComponent<PropShowingBehavior>();
        propShowingBehavior.HasProp(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!currentOwner)
        {
            if (other.TryGetComponent(out PlayerCrownHandling playerCrownHandling))
            {
                if (playerCrownHandling.GetComponent<Target>().IsDead())
                {
                    return;
                }
                playerCrownHandling.EquipCrown(this);
                currentOwner = playerCrownHandling;
                propShowingBehavior.enabled = false;
                lightCrown.intensity = 0.5f;
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void RemoveOwner()
    {
        if (currentOwner)
        {
            currentOwner = null;
            propShowingBehavior.enabled = true;
            lightCrown.intensity = 1.5f;
        }
        else
        {
            Debug.LogError("RemoveOwner: Crown Owner does not exist!");
        }
    }
}
