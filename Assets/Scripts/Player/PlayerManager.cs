using System;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerDeathBehavior playerDeath;
    private PlayerWeaponHandling weaponHandling;   
    private PlayerStatistics playerStatistics;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeath = GetComponent<PlayerDeathBehavior>();
        playerStatistics = GetComponent<PlayerStatistics>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
    }

    public void EndGame()
    {
        playerMovement.enabled = false;
        playerDeath.enabled = false;
        weaponHandling.enabled = false;
        playerStatistics.enabled = false;
    }

    public void StartGame()
    {
        playerMovement.enabled = true;
        playerDeath.enabled = true;
        weaponHandling.enabled = true;
        playerStatistics.enabled = true;
    }
    
    
}
