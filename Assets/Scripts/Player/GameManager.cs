using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    FFA,
    CrownChase,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private List<SpawnPointBehavior> spawnPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject lobbyCameraObject;
    
    [Header("GameMode Settings")]
    [SerializeField] private GameMode gameMode;
    [SerializeField] [Range(1f,300f)] private float gameModeDuration;
    [SerializeField] [Range(1,30)] private int gameModeScore;
    
    private GameModeBase currentGameMode;
    public GameModeBase CurrentGameMode => currentGameMode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerInputManager = GetComponent<PlayerInputManager>();
            InitializeGameMode();
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

    void Start()
    {
        spawnPoints = new List<SpawnPointBehavior>(FindObjectsOfType<SpawnPointBehavior>());
    }

    void Update()
    {
        currentGameMode.Update();
        Rotate();
    }

    public void OnPlayerJoined(PlayerInput obj)
    {
        currentGameMode.AddPlayerStatistic(obj);
        if (obj.playerIndex == 1)
        {
            currentGameMode.StartGame();
        }
        StartCoroutine(SetPlayerPositionAfterFrame(obj));
    }
    
    private IEnumerator SetPlayerPositionAfterFrame(PlayerInput obj)
    {
        yield return new WaitForEndOfFrame();

        DeactivateLobbyCamera(obj);

        if (obj.playerIndex < 0 || obj.playerIndex >= spawnPoints.Count)
        {
            Debug.LogError("Player index out of range");
            yield break;
        }
        yield return new WaitForEndOfFrame();
        obj.GetComponent<PlayerMovement>().SetPlayerPositionAndRotation(spawnPoints[obj.playerIndex].transform.position,Quaternion.identity);
        obj.GetComponent<PlayerMovement>().SetPlayerLayer((int)Mathf.Log(playerLayers[obj.playerIndex].value, 2));
    }

    private void DeactivateLobbyCamera(PlayerInput obj)
    {
        if (obj.playerIndex == 0)
        {
            lobbyCameraObject.SetActive(false);
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
                currentGameMode.onGameEnd.AddListener(HandleEndGame)
;               break;
            case GameMode.CrownChase:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    public float GetTimer()
    {
        return currentGameMode.Timer;
    }
}
