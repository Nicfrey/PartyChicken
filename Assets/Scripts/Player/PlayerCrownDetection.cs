using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrownDetection : MonoBehaviour
{
    private Crown crown;
    private PlayerCrownHandling playerCrownHandling;
    [SerializeField] 
    private Transform arrowTransform;

    private void Start()
    {
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
        if (GameManager.Instance.CurrentGameMode.GetType() != typeof(CaptureTheCrown))
        {
            enabled = false;
            arrowTransform.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        GetCrown();
        RotateDetection();
    }

    private void GetCrown()
    {
        if (!crown)
        {
            crown = FindObjectOfType<Crown>();
        }
    }

    private void RotateDetection()
    {
        if (!crown)
            return;
        
        
        if (playerCrownHandling.HasCrown)
        {
            arrowTransform.gameObject.SetActive(false);
        }
        else
        {
            arrowTransform.gameObject.SetActive(true);
            arrowTransform.forward = (crown.transform.position - transform.position).normalized;
        }
    }

    public void SetLayer(int layer)
    {
        arrowTransform.GetChild(0).gameObject.layer = layer;
    }
}
