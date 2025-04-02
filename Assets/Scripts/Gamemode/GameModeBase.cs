using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum GameModeState
{
    Starting,
    Playing,
    Paused,
    Ending,
}

public abstract class GameModeBase
{
    protected float timerGame;

    protected int scoreGoal;

    protected List<PlayerStatistics> players;
    
    public GameModeState State { get; protected set; }
    public float Timer => timerGame;

    public UnityEvent<PlayerStatistics> onGameEnd = new ();
    public UnityEvent<PlayerStatistics> onGameStart = new ();

    protected GameModeBase(float timerGame, int scoreGoal)
    {
        this.scoreGoal = scoreGoal;
        this.timerGame = timerGame;
        players = new();
        State = GameModeState.Starting;
    }
    
    public void AddPlayerStatistic(PlayerInput playerInput)
    {
        players.Add(playerInput.GetComponent<PlayerStatistics>());
    }

    public void StartGame()
    {
        State = GameModeState.Playing;
        foreach (PlayerStatistics player in players)
        {
            player.ResetStats();
        }
    }

    public void PauseGame()
    {
        State = GameModeState.Paused;
    }

    public int GetScoreGoal()
    {
        return scoreGoal;
    }

    public void Update()
    {
        if (State == GameModeState.Playing)
        {
            AddScore();
            CheckEndGame();
            HandleTimer();
        }
    }

    private void HandleTimer()
    {
        timerGame -= Time.deltaTime;
        if (timerGame <= 0)
        {
            State = GameModeState.Ending;
            // Check the player with the highest score
            int highestScore = players.Max(player => player.Score);
            List<PlayerStatistics> highestPlayers = players.Where(player => player.Score == highestScore).ToList();
            if (highestPlayers.Count > 1)
            {
                int highestDeath = highestPlayers.Max(player => player.Deaths);
                PlayerStatistics winner = highestPlayers.FirstOrDefault(player => player.Deaths == highestDeath);
                onGameEnd?.Invoke(winner);
            }
            else
            {
                onGameEnd?.Invoke(players.First());
            }
        }
    }

    protected abstract void CheckEndGame();
    protected abstract void AddScore();
}
