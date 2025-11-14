using System;
using System.Collections;
using System.Collections.Generic;
using Gamemode;
using Settings;
using UnityEngine;

public class ObjectiveDetection : MonoBehaviour
{
    private PlayerCrownHandling playerCrownHandling;
    private PlayerStatistics playerStatistics;
    private KingOfTheHill _kingOfTheHill;
    [SerializeField] private Transform arrowTransform;

    private void Start()
    {
        switch (GlobalSettings.Instance.CurrentGameMode)
        {
            case GameMode.FFA:
                HandleStartFFAMode();
                break;
            case GameMode.CrownChase:
                HandleStartCrownMode();
                break;
            case GameMode.KingOfTheHill:
                HandleStartKingOfTheHillMode();
                GameManager gameManager = FindObjectOfType<GameManager>();
                _kingOfTheHill = (KingOfTheHill)gameManager.CurrentGameMode;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        playerStatistics = GetComponent<PlayerStatistics>();
    }

    private void HandleStartCrownMode()
    {
        enabled = true;
        arrowTransform.gameObject.SetActive(true);
        playerCrownHandling = GetComponent<PlayerCrownHandling>();
    }

    private void HandleStartFFAMode()
    {
        enabled = false;
        arrowTransform.gameObject.SetActive(false);
    }

    private void HandleStartKingOfTheHillMode()
    {
        enabled = true;
        arrowTransform.gameObject.SetActive(true);
    }

    private void Update()
    {
        switch (GlobalSettings.Instance.CurrentGameMode)
        {
            case GameMode.FFA:
                break;
            case GameMode.CrownChase:
                RotateDetectionCrown();
                break;
            case GameMode.KingOfTheHill:
                RotateDetectionKOTH();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RotateDetectionCrown()
    {
        if (!Crown.Instance)
            return;


        if (playerCrownHandling.HasCrown)
        {
            arrowTransform.gameObject.SetActive(false);
        }
        else
        {
            arrowTransform.gameObject.SetActive(true);
            arrowTransform.LookAt(Crown.Instance.transform.position);
            arrowTransform.rotation = Quaternion.Euler(0, arrowTransform.rotation.eulerAngles.y, 0);
        }
    }

    private void RotateDetectionKOTH()
    {
        arrowTransform.gameObject.SetActive(!_kingOfTheHill.CurrentHill.IsPlayerInHill(playerStatistics));

        arrowTransform.LookAt(_kingOfTheHill.CurrentHill.transform.position);
        arrowTransform.rotation = Quaternion.Euler(0, arrowTransform.rotation.eulerAngles.y, 0);
    }

    public void SetLayer(int layer)
    {
        arrowTransform.GetChild(0).gameObject.layer = layer;
    }
}