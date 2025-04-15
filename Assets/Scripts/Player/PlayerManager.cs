using System;
using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerDeathBehavior playerDeath;
    private PlayerWeaponHandling weaponHandling;   
    private PlayerStatistics playerStatistics;
    private PlayerCrownHandling playerCrownHandling;
    private PlayerCrownDetection playerCrownDetection;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeath = GetComponent<PlayerDeathBehavior>();
        playerStatistics = GetComponent<PlayerStatistics>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
        playerCrownDetection = GetComponent<PlayerCrownDetection>();
        EndGame();
    }

    public void EndGame()
    {
        playerMovement.enabled = false;
        playerDeath.enabled = false;
        weaponHandling.enabled = false;
        playerStatistics.enabled = false;
        playerCrownHandling.enabled = false;
        playerCrownDetection.enabled = false;
    }

    public void StartGame()
    {
        playerMovement.enabled = true;
        playerDeath.enabled = true;
        weaponHandling.enabled = true;
        playerStatistics.enabled = true;
        playerCrownHandling.enabled = true;
        playerCrownDetection.enabled = true;
    }

    public void SetPlayerLayer(int layer)
    {
        gameObject.layer = layer;
        GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layer;
        GetComponentInChildren<Camera>().cullingMask |= 1 << layer;
        playerCrownDetection.SetLayer(layer);
    }
    
    
}
