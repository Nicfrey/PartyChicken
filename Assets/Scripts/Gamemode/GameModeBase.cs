using System.Collections.Generic;
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
    
    public GameModeState State { get; private set; }

    public UnityEvent<PlayerStatistics> onGameEnd;

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
    }

    public void PauseGame()
    {
        State = GameModeState.Paused;
    }

    public int GetScoreGoal()
    {
        return scoreGoal;
    }

    public abstract void CheckEndGame();
    protected abstract void AddScore();
}
