using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform aimCamera;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        aimCamera.forward = Vector3.Slerp(aimCamera.forward, characterTransform.forward, Time.deltaTime * rotationSpeed);
    }
}