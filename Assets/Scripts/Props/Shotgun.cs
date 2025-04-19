using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private float angleOpening = 45f;
    [SerializeField] private int bulletShot = 10;

    protected override void ShootSemiAutomatic(Vector3 direction, Vector3 origin)
    {
        if (canShoot)
        {
            base.ShootSemiAutomatic(direction, origin);
            --currentAmmunition;
            canShoot = false;
        }
    }

    protected override void ShootBehavior()
    {
        muzzleFlash.Play();
        for(int i = 0 ; i < bulletShot ; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint);
            bullet.transform.SetParent(null);
            BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
            Vector3 newDirection = GetRandomDirectionInCone(bulletSpawnPoint.forward, angleOpening);
            bulletBehaviour.SetDirection(newDirection);
            bulletBehaviour.StartMove = true;
            bulletBehaviour.SetupBullet(bulletSpeed, range, damage, owner);
            Debug.DrawRay(bulletSpawnPoint.position, newDirection * bulletSpeed, Color.red,2f);
        }
    }

    private Vector3 GetRandomDirectionInCone(Vector3 coneDirection, float angle)
    {
        float randomAngle = Random.Range(0f, angle * Mathf.Deg2Rad);
        float randomRadius = Mathf.Tan(randomAngle);
        Vector2 randomCircle = Random.insideUnitCircle * randomRadius;

        Vector3 localDir = new Vector3(
            randomCircle.x,
            randomCircle.y,
            1f 
        ).normalized;

        return Quaternion.FromToRotation(Vector3.forward, coneDirection.normalized) * localDir;
    }
}