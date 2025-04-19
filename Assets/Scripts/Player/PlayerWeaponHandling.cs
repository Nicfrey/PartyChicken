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
    
    private TwoBoneIKConstraint twoBoneIKConstraint;
    private Weapon currentWeapon = null;
    private bool isShooting = false;
    private Target target;

    private void Start()
    {
        twoBoneIKConstraint = weaponHolder.GetComponent<TwoBoneIKConstraint>();
        twoBoneIKConstraint.weight = HasWeapon() ? 1f : 0f;
        target = GetComponent<Target>();
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
            if (isShooting)
            {
                currentWeapon.Shoot(transform.forward, transform.position);
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
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!enabled || target.IsDead())
            return;
        
        if (HasWeapon())
        {
            isShooting = context.ReadValueAsButton();
            if (context.canceled)
                currentWeapon.StopShoot();
        }
        else
        {
            animator.SetBool("IsPunching",true);
            animator.SetFloat("Punch",Random.Range(0,1));
            Invoke(nameof(ResetPunch),0.5f);
        }
    }

    private void ResetPunch()
    {
        animator.SetBool("IsPunching",false);
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
        }
    }

    private bool HasWeapon()
    {
        return currentWeapon != null;
    }
}