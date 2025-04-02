using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerWeaponHandling : MonoBehaviour
{
    public UnityEvent<Weapon> onWeaponChange;

    public UnityEvent<Weapon> onWeaponThrow;

    public UnityEvent<Weapon> onWeaponShoot;

    [SerializeField] private Transform weaponHolder;
    private Weapon currentWeapon = null;
    private bool isShooting = false;

    void Update()
    {
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
        if (!enabled)
            return;
        
        if (HasWeapon())
        {
            isShooting = context.ReadValueAsButton();
            if (context.canceled)
                currentWeapon.StopShoot();
        }
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
            onWeaponThrow?.Invoke(currentWeapon);
            Destroy(currentWeapon.gameObject, 5f);
            currentWeapon = null;
        }
    }

    private bool HasWeapon()
    {
        return currentWeapon != null;
    }
}