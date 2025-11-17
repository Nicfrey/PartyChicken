using System;
using AI;
using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool isAIPlayer = false;
    public bool IsAIPlayer => isAIPlayer;
    [Header("Player Settings")]
    private PlayerMovement playerMovement;
    private PlayerDeathBehavior playerDeath;
    private PlayerWeaponHandling weaponHandling;   
    private PlayerStatistics playerStatistics;
    private PlayerCrownHandling playerCrownHandling;
    private ObjectiveDetection objectiveDetection;
    
    [Header("AI Settings")]
    private AiPlayerManager aiPlayerManager;
    

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDeath = GetComponent<PlayerDeathBehavior>();
        playerStatistics = GetComponent<PlayerStatistics>();
        weaponHandling = GetComponent<PlayerWeaponHandling>();
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
        objectiveDetection = GetComponent<ObjectiveDetection>();
        aiPlayerManager = GetComponent<AiPlayerManager>();
    }

    private void Start()
    {
        EndGame();
    }

    public void EndGame()
    {
        ActivationCommon(false);
        if(!isAIPlayer)
            ActivationPlayer(false);
        else 
            ActivationAI(false);
    }

    public void StartGame()
    {
        ActivationCommon();
        if (!isAIPlayer)
            ActivationPlayer();
        else
            ActivationAI();
    }

    private void ActivationAI(bool activate = true)
    {
        aiPlayerManager.ResetPath();
        aiPlayerManager.enabled = activate;
    }

    public void SetPlayerLayer(int layer)
    {
        gameObject.layer = layer;
        if(!isAIPlayer)
        {
            GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layer;
            GetComponentInChildren<Camera>().cullingMask |= 1 << layer;
            objectiveDetection.SetLayer(layer);
        }
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
    
    
}
