using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Crown : MonoBehaviour
{
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
        }
        else
        {
            Debug.LogError("RemoveOwner: Crown Owner does not exist!");
        }
    }
}
