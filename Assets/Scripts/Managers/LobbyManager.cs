using System;
using System.Collections.Generic;
using Cinemachine;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameSettingsUI;
        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] private GameObject cameraLobby;

        private List<PlayerLobbySelection> _playerSkinSelections = new();
        bool OnePlayerKeyboardJoined = false;
        
        public void ReturnToMainMenu()
        {
            DeactivateLobby();
            mainMenuUI.SetActive(true);
        }
        
        private void OpenGameSettings()
        {
            gameSettingsUI.SetActive(true);
        }

        private void DeactivateLobby()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if(!OnePlayerKeyboardJoined && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                JoinPlayerKeyboard();
            }
            
            if (_playerSkinSelections.Count >= 1)
            {
                int numberReady = GetNumberOfPlayersReady();

                if (numberReady == _playerSkinSelections.Count)
                {
                    GlobalSettings.Instance.SetPlayerSkins(_playerSkinSelections);
                    OpenGameSettings();
                }
            }
        }

        private int GetNumberOfPlayersReady()
        {
            int numberReady = 0;
            foreach (PlayerLobbySelection playerSelection in _playerSkinSelections)
            {
                if (playerSelection.Selected)
                {
                    numberReady++;
                }
            }
            return numberReady;
        }

        public void OnPlayerJoined(PlayerInput newPlayer)
        {
            DeactivateLobbyCamera(newPlayer);
            
            newPlayer.gameObject.layer =
                (int)Mathf.Log(GlobalSettings.Instance.PlayerLayers[newPlayer.playerIndex].value, 2);
            
            PlayerLobbySelection playerSelection = newPlayer.GetComponent<PlayerLobbySelection>();
            _playerSkinSelections.Add(playerSelection);
            playerSelection.GetSelectionPlayerUI();
            playerSelection.ActivateCameraPlayer();
            if (newPlayer.devices.Count > 0)
            {
                playerSelection.SelectedDevice = newPlayer.devices[0];
            }
        }

        public void JoinPlayerKeyboard()
        {
            if (playerInputManager.playerCount < playerInputManager.maxPlayerCount)
            {
                PlayerInput newPlayer = PlayerInput.Instantiate(playerInputManager.playerPrefab, playerIndex: playerInputManager.playerCount, splitScreenIndex: -1, pairWithDevice: null, controlScheme: "ControlScheme");
                newPlayer.SwitchCurrentControlScheme("ControlScheme", Keyboard.current, Mouse.current);
                newPlayer.GetComponent<PlayerLobbySelection>().IsKeyboard = true;
                OnePlayerKeyboardJoined = true;
            }
        }
        
        private void DeactivateLobbyCamera(PlayerInput newPlayer)
        {
            if (newPlayer.playerIndex == 0)
            {
                cameraLobby.GetComponentInChildren<CinemachineVirtualCamera>().Priority = 9;
            }
        }
    }
}