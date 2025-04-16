using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCrownHandling : MonoBehaviour
{
    [SerializeField] private Transform crownTransform;

    private Crown currentCrown;
    public bool HasCrown => currentCrown != null;

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
            currentCrown.transform.SetParent(null, true);
            if (playerShooting)
            {
                currentCrown.transform.position = transform.position;
                currentCrown.transform.rotation = Quaternion.identity;
                currentCrown.transform.position += Vector3.up;    
            }
            else
            {
                CrownSpawnerBehavior[] spawners = FindObjectsOfType<CrownSpawnerBehavior>();
                int randomSpawner = Random.Range(0, spawners.Length);
                CrownSpawnerBehavior spawner = spawners[randomSpawner];
                currentCrown.transform.position = spawner.transform.position;
            }
            
            currentCrown.RemoveOwner();
            currentCrown = null;
        }
    }
}