using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrownHandling : MonoBehaviour
{
    [SerializeField]
    private Transform crownTransform;
    
    private Crown currentCrown;

    private void Start()
    {
        GetComponent<Target>().onDeath.AddListener(RemoveCrown);
    }

    public void EquipCrown(Crown crown)
    {
        currentCrown = crown;
        crown.transform.SetParent(crownTransform);
        crown.transform.position = crownTransform.position;
        crown.transform.rotation = crownTransform.rotation;
    }

    public void RemoveCrown(PlayerStatistics playerShooting)
    {
        if (currentCrown)
        {
            currentCrown.transform.SetParent(null,true);
            currentCrown.transform.position = transform.position;
            currentCrown.transform.rotation = Quaternion.identity;
            currentCrown.transform.position += Vector3.up;
            currentCrown.RemoveOwner();
            currentCrown = null;
        }
    }
}
