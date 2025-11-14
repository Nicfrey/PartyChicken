using System;
using AI;
using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool isAIPlayer = false;
    [Header("Player Settings")]
    private PlayerMovement playerMovement;
    private PlayerDeathBehavior playerDeath;
    private PlayerWeaponHandling weaponHandling;   
    private PlayerStatistics playerStatistics;
    private PlayerCrownHandling playerCrownHandling;
    private ObjectiveDetection objectiveDetection;
    
    [Header("AI Settings")]
    private AIPlayerMovement aiPlayerMovement;
    private AIPropsDetection aiPropsDetection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeath = GetComponent<PlayerDeathBehavior>();
        playerStatistics = GetComponent<PlayerStatistics>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
        objectiveDetection = GetComponent<ObjectiveDetection>();
        aiPlayerMovement = GetComponent<AIPlayerMovement>();
        aiPropsDetection = GetComponent<AIPropsDetection>();
    }

    private void Start()
    {
        EndGame();
    }

    public void EndGame()
    {
        ActivationCommon(false);
        if (isAIPlayer)
        {
            ActivationAI(false);
        } 
        else
        {
            ActivationPlayer(false);
        }
    }

    public void StartGame()
    {
        ActivationCommon();
        if (isAIPlayer)
        {
            ActivationAI();
        } 
        else
        {
            ActivationPlayer();
        }
    }

    public void SetPlayerLayer(int layer)
    {
        gameObject.layer = layer;
        GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layer;
        GetComponentInChildren<Camera>().cullingMask |= 1 << layer;
        objectiveDetection.SetLayer(layer);
        weaponHandling.SetLayerCanvas(layer);
    }

    private void ActivationPlayer(bool activate = true)
    {
        playerMovement.enabled = activate;
        objectiveDetection.enabled = activate;
    }
    
    private void ActivationCommon(bool activate = true)
    {
        weaponHandling.enabled = activate;
        playerStatistics.enabled = activate;
        playerCrownHandling.enabled = activate;
        playerDeath.enabled = activate;
    }
    
    private void ActivationAI(bool activate = true)
    {
        aiPlayerMovement.enabled = activate;
        aiPropsDetection.enabled = activate;
    }
    
}
