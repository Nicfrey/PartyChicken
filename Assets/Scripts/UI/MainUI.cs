using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject mainMenuUI;
    
    private void Awake()
    {
        mainMenuUI.SetActive(true);
        lobbyUI.SetActive(false);
    }

    public void DisplayLobbyUI()
    {
        GameManager.Instance.ChangeState(GameState.Lobby);
        lobbyUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void DisplayMainMenuUI()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        lobbyUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
