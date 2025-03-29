using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] 
    private GameObject weaponUI;
    [SerializeField] 
    private TextMeshProUGUI nameWeapon;
    [SerializeField]
    private TextMeshProUGUI ammunition;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Image healthBar;

    private PlayerWeaponHandling weaponHolder;
    private Target target;
    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GetComponent<PlayerWeaponHandling>();
        target = GetComponent<Target>();
        weaponUI.SetActive(false);
        weaponHolder.onWeaponChange.AddListener(UpdateWeaponUI);
        weaponHolder.onWeaponThrow.AddListener(UpdateWeaponUI);
        weaponHolder.onWeaponShoot.AddListener(UpdateWeaponUI);
        target.onHealthChanged.AddListener(UpdateHealthUI);
    }

    public void SetTarget(Target newTarget)
    {
        target = newTarget;
    }

    public void SetWeaponHolder(PlayerWeaponHandling newWeaponHolder)
    {
        weaponHolder = newWeaponHolder;
    }

    void UpdateWeaponUI(Weapon weapon)
    {
        if (weapon == null)
        {
            weaponUI.SetActive(false);
        }
        else
        {
            weaponUI.SetActive(true);
            nameWeapon.text = weapon.GetName();
            ammunition.text = weapon.GetAmmunition() + " / " + weapon.GetTotalAmmunition();
        }
    }

    void OnDestroy()
    {
        weaponHolder.onWeaponChange.RemoveListener(UpdateWeaponUI);
        weaponHolder.onWeaponThrow.RemoveListener(UpdateWeaponUI);
        weaponHolder.onWeaponShoot.RemoveListener(UpdateWeaponUI);
        target.onHealthChanged.RemoveListener(UpdateHealthUI);
    }

    private void UpdateHealthUI(int health)
    {
        float percentage = (float)health / target.GetMaxHealth();
        healthBar.fillAmount = percentage;
        healthText.text = health.ToString();
    }
}
