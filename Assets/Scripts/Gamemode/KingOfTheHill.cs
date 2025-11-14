using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Gamemode
{
    public class KingOfTheHill : GameModeBase
    {
        private HillZone[] hillZones;
        private int currentHillIndex = 0;
        public HillZone CurrentHill => hillZones[currentHillIndex];
        private Dictionary<PlayerStatistics, float> playerHillTimes = new Dictionary<PlayerStatistics, float>();

        public KingOfTheHill(float timerGame, int scoreGoal) : base(timerGame, scoreGoal)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            hillZones = Object.FindObjectsOfType<HillZone>(true);
            var rng = new System.Random();
            rng.ShuffleArray(hillZones);
            currentHillIndex = 0;
            CurrentHill.SetActive(true);
            foreach(var player in players)
            {
                playerHillTimes[player] = 0f;
            }
        }


        protected override void CheckEndGame()
        {
            foreach (PlayerStatistics statistics in players)
            {
                if (statistics.Score >= scoreGoal)
                {
                    onGameEnd?.Invoke(statistics);
                    State = GameModeState.Ending;
                }
            }
        }

        protected override void AddScore()
        {
            if (!CurrentHill.HasOnlyOnePlayerInHill())
                return;
            
            foreach (var player in players)
            {
                if (CurrentHill.IsPlayerInHill(player))
                {
                    playerHillTimes[player] += Time.deltaTime;
                    player.Score = Mathf.FloorToInt(playerHillTimes[player]);
                }
            }
        }
    }
}