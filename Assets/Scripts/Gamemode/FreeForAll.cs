using UnityEngine;
using UnityEngine.InputSystem;

public class FreeForAll : GameModeBase
{
    public FreeForAll(float timerGame, int scoreGoal) : base(timerGame, scoreGoal)
    {
        
    }

    public override void CheckEndGame()
    {
        AddScore();
        foreach (PlayerStatistics statistics in players)
        {
            if (statistics.Kills >= scoreGoal)
            {
                onGameEnd.Invoke(statistics);
            }
        }
    }

    protected override void AddScore()
    {
        foreach (PlayerStatistics statistics in players)
        {
            statistics.Score = statistics.Kills;
        }
    }
}
