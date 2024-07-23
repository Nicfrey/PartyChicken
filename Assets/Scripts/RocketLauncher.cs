using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    [SerializeField] 
    private GameObject fakeRocketObject;

    protected override void ShootSemiAutomatic(Vector3 direction, Vector3 origin)
    {
        base.ShootSemiAutomatic(direction, origin);
        fakeRocketObject.SetActive(false);
    }
}
