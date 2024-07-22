using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandling : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHolder;
    private Weapon currentWeapon = null;

    public void EquipWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
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
        if (currentWeapon != null)
        {
            if(context.performed) 
                currentWeapon.Shoot(transform.forward, transform.position);
            if(context.canceled)
                currentWeapon.StopShoot();
        }
    }

    public void Throw()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Throw(transform.forward, transform.position);
            Destroy(currentWeapon.gameObject);
        }
    }
}
