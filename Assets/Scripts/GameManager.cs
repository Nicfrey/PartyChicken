using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Gamemode;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public enum GameState
{
    PauseMenu,
    Playing,
    StartPlaying
}

public class GameManager : MonoBehaviour
{
    private List<SpawnPointBehavior> spawnPoints = new();
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject lobbyCameraObject;

    [Header("Prefabs for players")] [SerializeField]
    private GameObject playerPrefab;

    [Header("GameMode Settings")] [SerializeField]
    private GameMode gameMode;

    [SerializeField] private GameObject crownPrefab;
    private GameState gameState;
    private GameModeBase currentGameMode;
    public GameModeBase CurrentGameMode => currentGameMode;

    [Header("Debug")] [SerializeField] private bool debugMode;
    [SerializeField] private GameState gameStateDebug;

    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        ChangeState(debugMode ? gameStateDebug : GameState.StartPlaying);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (gameState == GameState.Playing || gameState == GameState.StartPlaying)
        {
            currentGameMode.Update();
        }

        Rotate();
    }

    public void OnPlayerJoined(PlayerInput obj)
    {
        if (gameState == GameState.StartPlaying)
        {
            currentGameMode.AddPlayerStatistic(obj);
        }

        StartCoroutine(SetPlayerPositionAfterFrame(obj));
    }

    private IEnumerator SetPlayerPositionAfterFrame(PlayerInput obj)
    {
        yield return null;

        DeactivateLobbyCamera(obj);

        if (obj.playerIndex < 0)
        {
            Debug.LogError("Player index out of range");
            yield break;
        }

        obj.gameObject.layer = (int)Mathf.Log(GlobalSettings.Instance.PlayerLayers[obj.playerIndex].value, 2);
        obj.GetComponent<PlayerMovement>()
            .SetPlayerPositionAndRotation(spawnPoints[obj.playerIndex].transform.position, Quaternion.identity);
        obj.GetComponent<PlayerManager>()
            .SetPlayerLayer((int)Mathf.Log(GlobalSettings.Instance.PlayerLayers[obj.playerIndex].value, 2));
        obj.GetComponent<PlayerSkinSelection>()
            .SelectSkin(debugMode ? 0 : GlobalSettings.Instance.SkinSelected[obj.playerIndex].SkinSelected);
    }

    private void DeactivateLobbyCamera(PlayerInput obj)
    {
        if (obj.playerIndex == 0)
        {
            // lobbyCameraObject?.SetActive(false);
            GetComponentInChildren<CinemachineVirtualCamera>().Priority = 9;
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    }

    private void InitializeGameMode()
    {
        switch (GlobalSettings.Instance.CurrentGameMode)
        {
            case GameMode.FFA:
                currentGameMode = new FreeForAll(GlobalSettings.Instance.MaxTime, GlobalSettings.Instance.ScoreGoal);
                break;
            case GameMode.CrownChase:
                currentGameMode = new CaptureTheCrown(GlobalSettings.Instance.MaxTime,
                    GlobalSettings.Instance.ScoreGoal, crownPrefab);
                break;
            case GameMode.KingOfTheHill:
                currentGameMode = new KingOfTheHill(GlobalSettings.Instance.MaxTime,
                    GlobalSettings.Instance.ScoreGoal);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        currentGameMode.onGameEnd.AddListener(HandleEndGame);
        currentGameMode.onGameStart.AddListener(() => ChangeState(GameState.Playing));
    }

    public int GetScoreGoal()
    {
        return currentGameMode.GetScoreGoal();
    }

    private void HandleEndGame(PlayerStatistics winner)
    {
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in players)
        {
            player.EndGame();
        }
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        if (newState == GameState.PauseMenu)
        {
            Time.timeScale = 0f;
            playerInputManager.enabled = false;
        }
        else if (newState == GameState.Playing)
        {
            Time.timeScale = 1f;
            if (playerInputManager)
                playerInputManager.enabled = false;
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager player in players)
            {
                player.StartGame();
            }
        }
        else if (newState == GameState.StartPlaying)
        {
            playerInputManager.enabled = true;
            InitializeGameMode();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        if (gameState == GameState.StartPlaying)
        {
            spawnPoints.Clear();
            spawnPoints = new List<SpawnPointBehavior>(FindObjectsOfType<SpawnPointBehavior>());
            playerInputManager.joinBehavior = debugMode
                ? PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed
                : PlayerJoinBehavior.JoinPlayersManually;
            if (!debugMode)
            {
                for (int i = 0; i < GlobalSettings.Instance.SkinSelected.Count; i++)
                {
                    playerInputManager.JoinPlayer(i, -1, null, GlobalSettings.Instance.SkinSelected[i].SelectedDevice);
                }
            }
        }
    }

    public float GetTimer()
    {
        return currentGameMode.Timer;
    }
}