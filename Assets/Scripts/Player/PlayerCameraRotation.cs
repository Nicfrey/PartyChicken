using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRotation : MonoBehaviour
{
    [Header("Component References")] [SerializeField]
    private Transform characterTransform;

    [SerializeField] private Transform aimCamera;
    private PlayerWeaponHandling playerWeaponHandling;

    [Header("Settings")] 
    [SerializeField] private float aimOffset;
    [SerializeField] private float walkOffset;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed = 10f;

    private void Start()
    {
        playerWeaponHandling = GetComponent<PlayerWeaponHandling>();
    }

    private void Update()
    {
        aimCamera.localPosition = Vector3.Lerp(aimCamera.localPosition, GetAimDirection(), zoomSpeed * Time.deltaTime);
        aimCamera.forward =
            Vector3.Slerp(aimCamera.forward, characterTransform.forward, Time.deltaTime * rotationSpeed);
    }

    private Vector3 GetAimDirection()
    {
        Vector3 direction = aimCamera.forward;
        direction.y = 0;
        direction *= playerWeaponHandling.IsAiming() ? aimOffset : walkOffset;
        return direction;
    }
}