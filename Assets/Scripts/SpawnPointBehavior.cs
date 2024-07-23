using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehavior : MonoBehaviour
{
    [SerializeField] 
    private float radiusDetectionPlayer;

    private bool isAvailable = true;
    
    void Start()
    {
        InvokeRepeating("CheckAvailability",0f,1f);
    }

    private void CheckAvailability()
    {
        Collider[] objectsCollided = Physics.OverlapSphere(transform.position, radiusDetectionPlayer);
        foreach (Collider objectCollided in objectsCollided)
        {
            if (objectCollided.GetComponent<CharacterController>() != null)
            {
                isAvailable = false;
                return;
            }
        }
        isAvailable = true;
    }

    public bool IsAvailable()
    {
        return isAvailable;
    }

    void OnDisable()
    {
        CancelInvoke("CheckAvailability");
    }

    public void SetUnavailable()
    {
        isAvailable = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusDetectionPlayer);
    }
}
