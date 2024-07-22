using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWeapon : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponPrefabs;

    [SerializeField] private float minSpawnRate = 5.0f;
    [SerializeField] private float maxSpawnRate = 10.0f;
    private float spawnRate;
    private WeaponHolder[] weaponHolders;


    // Start is called before the first frame update
    void Start()
    {
        weaponHolders = Object.FindObjectsOfType<WeaponHolder>();
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate <= 0)
        {
            SpawnWeapon();
            spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    private void SpawnWeapon()
    {
        if (!CanSpawnWeapon())
        {
            return;
        }

        bool weaponSpawned = false;
        while (!weaponSpawned)
        {
            int randomHolder = Random.Range(0, weaponHolders.Length);
            int randomWeapon = Random.Range(0, weaponPrefabs.Count);
            WeaponHolder weaponHolder = weaponHolders[randomHolder];
            if (!weaponHolder.HasWeapon())
            {
                GameObject weapon = Instantiate(weaponPrefabs[randomWeapon], weaponHolder.transform.position, weaponHolder.transform.rotation);
                weaponHolder.SetWeapon(weapon);
                weaponSpawned = true;
            }
        }
    }

    private bool CanSpawnWeapon()
    {
        foreach (WeaponHolder weaponHolder in weaponHolders)
        {
            if (!weaponHolder.HasWeapon())
            {
                return true;
            }
        }
        return false;
    }
}
