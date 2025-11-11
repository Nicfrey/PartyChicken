using System;
using UnityEngine;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private GameObject optionsUI;

        public void GoToLobby()
        {
            DeactivateMenu();
            lobbyUI.SetActive(true);
        }

        public void GoToOptions()
        {
            DeactivateMenu();
            optionsUI.SetActive(true);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
        
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void DeactivateMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
