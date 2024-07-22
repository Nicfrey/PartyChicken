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

    private int currentAmmunition;
    protected bool canShoot = true;

    void Awake()
    {
        currentAmmunition = ammunition;
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
        if(!HasNoAmmunition())
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
        currentAmmunition--;
        Debug.Log("Current ammunition: " + currentAmmunition);
        muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint);
        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.SetDirection(bulletSpawnPoint.forward);
        bulletBehaviour.StartMove = true;
        bulletBehaviour.Range = range;
        bulletBehaviour.Speed = 50f;
        if (!IsAutomatic() || HasNoAmmunition()) 
            canShoot = false;
    }

    private bool HasNoAmmunition()
    {
        return currentAmmunition <= 0;
    }

    private bool IsAutomatic()
    {
        return fireMode == FireMode.Automatic;
    }
}
