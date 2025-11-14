using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public enum GameMode
    {
        FFA,
        CrownChase,
        KingOfTheHill,
    }

    
    public class GlobalSettings : MonoBehaviour
    {
        public static GlobalSettings Instance { get; private set; }
        
        [SerializeField] private List<LayerMask> playerLayers;
        public List<LayerMask> PlayerLayers => playerLayers;
        public List<PlayerLobbySelection> SkinSelected { get; set; } = new();

        [Header("Debug Settings")] 
        [SerializeField] private bool useSerializedSettings = false;
        [SerializeField] private GameMode currentGameMode;
        [SerializeField] private int scoreGoal = 10;
        [SerializeField] private int maxTime = 300;
        public GameMode CurrentGameMode { get; set; }
        public int ScoreGoal { get; set; }
        public int MaxTime { get; set; }
        public int LevelSelected { get; set; }
        

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (!useSerializedSettings) 
                return;
            
            CurrentGameMode = currentGameMode;
            ScoreGoal = scoreGoal;
            MaxTime = maxTime;
        }

        public void SetPlayerSkins(List<PlayerLobbySelection> playerSkinSelections)
        {
            SkinSelected.Clear();
            foreach (var playerLobbySelection in playerSkinSelections)
            {
                SkinSelected.Add(playerLobbySelection);
            }
        }
    }
}
