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
    [SerializeField] private GameMode gameMode;
    
    private GameModeBase currentGameMode;
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
        currentGameMode.CheckEndGame();
        Rotate();
    }

    public void OnPlayerJoined(PlayerInput obj)
    {
        currentGameMode.AddPlayerStatistic(obj);
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
        Debug.Log("Player joined at index " + obj.playerIndex);
        Debug.Log("Player spawnpoint " + spawnPoints[obj.playerIndex].transform.position);
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
                currentGameMode = new FreeForAll(300f, 10);
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
}
