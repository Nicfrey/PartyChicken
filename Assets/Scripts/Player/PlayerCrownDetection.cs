using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrownDetection : MonoBehaviour
{
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
        RotateDetection();
    }

    private void GetCrown()
    {
    }

    private void RotateDetection()
    {
        if (!Crown.Instance)
            return;
        
        
        if (playerCrownHandling.HasCrown)
        {
            arrowTransform.gameObject.SetActive(false);
        }
        else
        {
            arrowTransform.gameObject.SetActive(true);
            arrowTransform.LookAt(Crown.Instance.transform.position);
            arrowTransform.rotation = Quaternion.Euler(0,arrowTransform.rotation.eulerAngles.y,0);
        }
    }

    public void SetLayer(int layer)
    {
        arrowTransform.GetChild(0).gameObject.layer = layer;
    }
}
