using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerWeaponHandling : MonoBehaviour
{
    public UnityEvent<Weapon> onWeaponChange;

    public UnityEvent<Weapon> onWeaponThrow;

    public UnityEvent<Weapon> onWeaponShoot;

    [SerializeField] private Transform weaponHolder;

    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;

    private TwoBoneIKConstraint twoBoneIKConstraint;
    private Weapon currentWeapon = null;
    private bool isShooting = false;
    private bool isAiming = false;
    private Target target;

    private void Start()
    {
        twoBoneIKConstraint = weaponHolder.GetComponent<TwoBoneIKConstraint>();
        twoBoneIKConstraint.weight = HasWeapon() ? 1f : 0f;
        target = GetComponent<Target>();
        canvas.gameObject.SetActive(false);
        target.onDeath.AddListener(OnDeath);
    }

    private void OnDeath(PlayerStatistics arg0)
    {
        Throw();
    }

    void Update()
    {
        twoBoneIKConstraint.weight = HasWeapon() ? 1f : 0f;
        if (target.IsDead())
            return;

        if (HasWeapon())
        {
            UpdateCanvasAim();
            if (isShooting)
            {
                currentWeapon.Shoot(
                    (canvas.transform.position - currentWeapon.muzzleFlash.transform.position).normalized,
                    transform.position);
                onWeaponShoot?.Invoke(currentWeapon);
            }
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        Throw();
        GameObject newWeapon = Instantiate(weapon, weaponHolder);
        newWeapon.transform.SetParent(weaponHolder.transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        currentWeapon.SetOwner(GetComponent<PlayerStatistics>());
        onWeaponChange?.Invoke(currentWeapon);
        canvas.gameObject.SetActive(true);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!enabled || target.IsDead())
        {
            isShooting = false;
            return;
        }

        if (HasWeapon())
        {
            isShooting = context.ReadValueAsButton();
            if (context.canceled)
                currentWeapon.StopShoot();
        }
        else
        {
            animator.SetBool("IsPunching", true);
            animator.SetFloat("Punch", Random.Range(0, 1));
            Invoke(nameof(ResetPunch), 0.5f);
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (!enabled || target.IsDead() || !HasWeapon())
        {
            isAiming = false;
            return;
        }
        
        isAiming = context.performed;
    }

    private void ResetPunch()
    {
        animator.SetBool("IsPunching", false);
    }

    public void ThrowInput(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;

        if (context.performed)
        {
            Throw();
        }
    }

    private void Throw()
    {
        if (HasWeapon())
        {
            currentWeapon.Throw(weaponHolder.forward, weaponHolder.position);
            onWeaponThrow?.Invoke(null);
            Destroy(currentWeapon.gameObject, 5f);
            currentWeapon = null;
            canvas.gameObject.SetActive(false);
        }
    }

    private bool HasWeapon()
    {
        return currentWeapon;
    }

    private void UpdateCanvasAim()
    {
        RaycastHit hit;
        float distance = Vector3.Distance(weaponHolder.position, canvas.transform.position);
        int mask = ~((1 << gameObject.layer) | (1 << LayerMask.NameToLayer("Weapon")) |
                     (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Objective")));
        if (Physics.Raycast(weaponHolder.position, weaponHolder.forward, out hit, distance, mask))
        {
            canvas.transform.position = hit.point;
        }
        else
        {
            canvas.transform.localPosition = Vector3.forward * 5f;
        }
    }

    public void SetLayerCanvas(int layer)
    {
        canvas.gameObject.layer = layer;
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}