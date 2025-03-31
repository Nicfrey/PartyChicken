using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarBehavior : MonoBehaviour
{
    [SerializeField] 
    private Image progressImage;

    private float percentage;

    public void SetPercentage(float percentage)
    {
        this.percentage = percentage;
        progressImage.fillAmount = this.percentage;
    }
    
}
