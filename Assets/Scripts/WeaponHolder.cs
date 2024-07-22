using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHolder;
    private Light weaponLight;
    private bool hasWeapon = false;

    private float startY;
    private float angle;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float amplitude = 0.5f;
    [SerializeField]
    private float rotationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        weaponLight = GetComponentInChildren<Light>();
        startY = weaponHolder.position.y;
        weaponLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndRotateWeapon();
    }

    private void MoveAndRotateWeapon()
    {
        if (hasWeapon)
        {
            angle += Time.deltaTime;
            float newY = startY + Mathf.Sin(angle * speed) * amplitude;
            weaponHolder.transform.position = new Vector3(weaponHolder.transform.position.x, newY, weaponHolder.transform.position.z);
            weaponHolder.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * rotationSpeed);
        }
    }

    public bool HasWeapon()
    {
        return hasWeapon;
    }

    public void SetWeapon(GameObject weapon)
    {
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weaponLight.enabled = true;
        hasWeapon = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if(!hasWeapon)
            return;

        if (other.GetComponent<PlayerWeaponHandling>())
        {
            PlayerWeaponHandling playerWeaponHandling = other.GetComponent<PlayerWeaponHandling>();
            playerWeaponHandling.EquipWeapon(weaponHolder.GetChild(0).gameObject);
            RemoveWeapon();
        }
    }

    private void RemoveWeapon()
    {
        GameObject weaponObject = weaponHolder.GetChild(0).gameObject;
        weaponObject.transform.SetParent(null);
        Destroy(weaponObject);
        weaponLight.enabled = false;
        hasWeapon = false;
    }
}
