using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    protected override void ShootSemiAutomatic(Vector3 direction, Vector3 origin)
    {
        if (canShoot)
        {
            if(Physics.Raycast(origin, direction,out RaycastHit hit, range))
            {
                if (hit.collider.TryGetComponent(out Target component))
                {
                    component.TakeDamage(damage);
                }
            }

            base.ShootSemiAutomatic(direction, origin);
        }
    }
}
  