using System.Collections.Generic;
using UnityEngine;

namespace Gamemode
{
    public class KingOfTheHill : GameModeBase
    {
        
        private List<PlayerStatistics> playersInHill = new();
        public KingOfTheHill(float timerGame, int scoreGoal) : base(timerGame, scoreGoal)
        {
            
        }

        protected override void CheckEndGame()
        {
            
        }

        protected override void AddScore()
        {
            
        }
    }
}
