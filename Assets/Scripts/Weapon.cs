using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum FireMode
    {
        Automatic,
        SemiAutomatic,
        Burst
    }

    public enum WeaponName
    {
        Pistol,
        Shotgun,
        AssaultRifle,
        SniperRifle,
        RocketLauncher
    }

    [SerializeField]
    protected int ammunition;
    [SerializeField] 
    protected int damage;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected FireMode fireMode;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    protected ParticleSystem muzzleFlash;
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected Transform bulletSpawnPoint;
    [SerializeField]
    protected float bulletSpeed;
    [SerializeField]
    protected WeaponName weaponName;

    private int currentAmmunition;
    protected bool canShoot = true;
    private float timerFire;

    void Awake()
    {
        currentAmmunition = ammunition;
        timerFire = fireRate;
    }

    void Update()
    {
        if (HasNoAmmunition())
        {
            return;
        }

        if (!canShoot)
        {
            timerFire -= Time.deltaTime;
            if (timerFire <= 0)
            {
                canShoot = true;
                timerFire = fireRate;
            }
        }
    }

    public void Shoot(Vector3 direction, Vector3 origin)
    {
        switch (fireMode)
        {
            case FireMode.Automatic:
                ShootAutomatic(direction, origin);
                break;
            case FireMode.SemiAutomatic:
                ShootSemiAutomatic(direction, origin);
                break;
            case FireMode.Burst:
                ShootBurst(direction, origin);
                break;
        }
    }

    public void StopShoot()
    {
        if(!HasNoAmmunition() && IsFireRateDone())
            canShoot = true;
    }

    public virtual void Throw(Vector3 direction, Vector3 origin)
    {

    }

    protected virtual void ShootAutomatic(Vector3 direction, Vector3 origin)
    {
        CommonTask();
    }

    protected virtual void ShootSemiAutomatic(Vector3 direction, Vector3 origin)
    {
        CommonTask();

    }

    protected virtual void ShootBurst(Vector3 direction, Vector3 origin)
    {
        CommonTask();
    }

    private void CommonTask()
    {
        ShootBehavior();
        // TODO fake recoil effect
    }

    private bool HasNoAmmunition()
    {
        return currentAmmunition <= 0;
    }

    private bool IsAutomatic()
    {
        return fireMode == FireMode.Automatic;
    }

    private void ShootBehavior()
    {
        if (canShoot)
        {
            Debug.Log("Shoot");
            currentAmmunition--;
            muzzleFlash.Play();
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint);
            bullet.transform.SetParent(null);
            BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
            bulletBehaviour.SetDirection(bulletSpawnPoint.forward);
            bulletBehaviour.StartMove = true;
            bulletBehaviour.SetupBullet(bulletSpeed, range, damage);
            canShoot = false;
        }
    }

    private bool IsFireRateDone()
    {
        return timerFire <= 0;
    }

    public int GetAmmunition()
    {
        return currentAmmunition;
    }

    public int GetTotalAmmunition()
    {
        return ammunition;
    }

    public String GetName()
    {
        switch (weaponName)
        {
            case WeaponName.Pistol:
                return "Pistol";
            case WeaponName.Shotgun:
                return "Shotgun";
            case WeaponName.AssaultRifle:
                return "Assault Rifle";
                break;
            case WeaponName.SniperRifle:
                return "Sniper Rifle";
                break;
            case WeaponName.RocketLauncher:
                return "Rocket Launcher";
                break;
            default:
                return "Unknown";
        }
    }
}
