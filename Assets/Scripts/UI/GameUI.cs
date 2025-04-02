using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] 
    private TMP_Text timerText;
    [SerializeField] 
    private TMP_Text playerWinningText;

    [SerializeField] 
    private GameObject endGameScreen;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.CurrentGameMode.onGameEnd.AddListener(HandleEndGame);
        endGameScreen.SetActive(false);
    }

    private void HandleEndGame(PlayerStatistics arg0)
    {
        endGameScreen.SetActive(true);
        playerWinningText.text = LayerMask.LayerToName(arg0.gameObject.layer) + " wins!";
    }

    private void Update()
    {
        HandleTimerText();
    }

    private void HandleTimerText()
    {
        float timer = gameManager.GetTimer();
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        string minutesStr = minutes < 10 ? "0" + minutes : minutes.ToString();
        string secondsStr = seconds < 10 ? "0" + seconds : seconds.ToString();
        timerText.text = minutesStr + ":" + secondsStr;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
