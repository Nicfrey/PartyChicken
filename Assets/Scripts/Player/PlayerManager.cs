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
    private ObjectiveDetection objectiveDetection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeath = GetComponent<PlayerDeathBehavior>();
        playerStatistics = GetComponent<PlayerStatistics>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
        objectiveDetection = GetComponent<ObjectiveDetection>();
    }

    private void Start()
    {
        EndGame();
    }

    public void EndGame()
    {
        playerMovement.enabled = false;
        playerDeath.enabled = false;
        weaponHandling.enabled = false;
        playerStatistics.enabled = false;
        playerCrownHandling.enabled = false;
        objectiveDetection.enabled = false;
    }

    public void StartGame()
    {
        playerMovement.enabled = true;
        playerDeath.enabled = true;
        weaponHandling.enabled = true;
        playerStatistics.enabled = true;
        playerCrownHandling.enabled = true;
        objectiveDetection.enabled = true;
    }

    public void SetPlayerLayer(int layer)
    {
        gameObject.layer = layer;
        GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layer;
        GetComponentInChildren<Camera>().cullingMask |= 1 << layer;
        objectiveDetection.SetLayer(layer);
        weaponHandling.SetLayerCanvas(layer);
    }
    
    
}
