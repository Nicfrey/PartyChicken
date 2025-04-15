using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameMode
{
    FFA,
    CrownChase,
}

public enum GameState
{
    MainMenu,
    PauseMenu,
    Playing,
    Settings,
    Lobby,
    StartPlaying
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private List<SpawnPointBehavior> spawnPoints = new();
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject lobbyCameraObject;
    
    [Header("Prefabs for players")]
    [SerializeField] private GameObject selectionPrefab;
    [SerializeField] private GameObject playerPrefab;
    [Header("GameMode Settings")]
    [SerializeField] private GameMode gameMode;
    [SerializeField] [Range(1f,300f)] private float gameModeDuration;
    [SerializeField] [Range(1,60)] private int gameModeScore;
    [SerializeField] private GameObject crownPrefab;
    private List<PlayerLobbySelection> playerSkinSelections = new();
    private GameState gameState;
    
    private GameModeBase currentGameMode;
    public GameModeBase CurrentGameMode => currentGameMode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.enabled = false;
            ChangeState(GameState.MainMenu);
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Update()
    {
        if (gameState == GameState.Playing || gameState == GameState.StartPlaying)
        {
            currentGameMode.Update();
        } 
        else if (gameState == GameState.Lobby)
        {
            HandleLobbyState();
        }
        Rotate();
    }

    private void HandleLobbyState()
    {
        if (playerSkinSelections.Count > 1)
        {
            int numberReady = 0;
            foreach (PlayerLobbySelection playerSelection in playerSkinSelections)
            {
                if (playerSelection.Selected)
                {
                    numberReady++;
                }
            }

            if (numberReady >= playerSkinSelections.Count)
            {
                ChangeState(GameState.StartPlaying);
            }
        }
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

        obj.gameObject.layer = (int)Mathf.Log(playerLayers[obj.playerIndex].value, 2);
        if (gameState == GameState.Lobby)
        {
            PlayerLobbySelection playerSelection = obj.GetComponent<PlayerLobbySelection>();
            playerSkinSelections.Add(playerSelection);
            playerSelection.GetSelectionPlayerUI();
        }
        else
        {
            obj.GetComponent<PlayerMovement>().SetPlayerPositionAndRotation(spawnPoints[obj.playerIndex].transform.position,Quaternion.identity);
            obj.GetComponent<PlayerManager>().SetPlayerLayer((int)Mathf.Log(playerLayers[obj.playerIndex].value, 2));
            obj.GetComponent<PlayerSkinSelection>().SelectSkin(playerSkinSelections[obj.playerIndex].SkinSelected);
        }
    }

    private void DeactivateLobbyCamera(PlayerInput obj)
    {
        if (gameState == GameState.StartPlaying)
        {
            if (obj.playerIndex == 0)
            {
                lobbyCameraObject?.SetActive(false);
            }
        }
    }

    private void Rotate()
    {
        if (playerInputManager.playerCount < 1)
        {
            transform.Rotate(Vector3.up, 10f * Time.deltaTime);
        }
    }

    private void InitializeGameMode()
    {
        switch (gameMode)
        {
            case GameMode.FFA:
                currentGameMode = new FreeForAll(gameModeDuration, gameModeScore);
                ;
                break;
            case GameMode.CrownChase:
                currentGameMode = new CaptureTheCrown(gameModeDuration, gameModeScore, crownPrefab);
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
        Debug.Log($"{LayerMask.LayerToName(winner.gameObject.layer)} won");
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in players)
        {
            player.EndGame();
        }
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        if (newState == GameState.Lobby)
        {
            playerInputManager.enabled = true;
            playerInputManager.splitScreen = false;
            playerInputManager.playerPrefab = selectionPrefab;
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            playerSkinSelections.Clear();
        }
        else if (newState == GameState.MainMenu)
        {
            playerInputManager.enabled = false;
        }
        else if (newState == GameState.PauseMenu)
        {
            Time.timeScale = 0f;
            playerInputManager.enabled = false;
        }
        else if (newState == GameState.Playing)
        {
            Time.timeScale = 1f;
            playerInputManager.enabled = false;
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager player in players)
            {
                player.StartGame();
            }
        }
        else if (newState == GameState.Settings)
        {
            
        }
        else if (newState == GameState.StartPlaying)
        {
            InitializeGameMode();
            SceneManager.LoadScene("SecondLevel");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameState == GameState.StartPlaying)
        {
            spawnPoints.Clear();
            spawnPoints = new List<SpawnPointBehavior>(FindObjectsOfType<SpawnPointBehavior>());
            playerInputManager.playerPrefab = playerPrefab;
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            playerInputManager.splitScreen = true;
            foreach (PlayerLobbySelection playerSelection in playerSkinSelections)
            {
                Instantiate(playerPrefab);
            }
        }
    }

    public float GetTimer()
    {
        return currentGameMode.Timer;    
    }
}
