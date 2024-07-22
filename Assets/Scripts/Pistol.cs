using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    protected override void ShootSemiAutomatic(Vector3 direction, Vector3 origin)
    {
        if (canShoot)
        {
            base.ShootSemiAutomatic(direction, origin);
        }
    }
}
  