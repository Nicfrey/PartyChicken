using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandling : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHolder;
    private Weapon currentWeapon = null;
    private bool isShooting = false;

    void Update()
    {
        if (HasWeapon())
        {
            if (isShooting)
                currentWeapon.Shoot(transform.forward, transform.position);
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (HasWeapon())
        {
            currentWeapon.Throw(transform.forward,transform.position);
            Destroy(currentWeapon.gameObject);
        }
        GameObject newWeapon = Instantiate(weapon, weaponHolder);
        newWeapon.transform.SetParent(weaponHolder.transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (HasWeapon())
        {
            isShooting = context.ReadValueAsButton();
            if (context.canceled)
                currentWeapon.StopShoot();
        }
    }

    public void Throw()
    {
        if (HasWeapon())
        {
            currentWeapon.Throw(transform.forward, transform.position);
            Destroy(currentWeapon.gameObject);
        }
    }

    private bool HasWeapon()
    {
        return currentWeapon != null;
    }
}
