using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private PlayerInputManager playerInputManager;

    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        if (obj.playerIndex < 0 || obj.playerIndex >= spawnPoints.Count)
        {
            Debug.LogError("Player index out of range");
            return;
        }
        obj.transform.position = spawnPoints[obj.playerIndex].position;
        obj.transform.rotation = spawnPoints[obj.playerIndex].rotation;
        obj.GetComponent<PlayerMovement>().SetPlayerLayer((int)Mathf.Log(playerLayers[obj.playerIndex].value, 2));
    }

    void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
}
