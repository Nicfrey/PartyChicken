using System.Collections.Generic;
using System.Globalization;
using Settings;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class GameSettingsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown gameModeDropdown;
        [SerializeField] private TMP_Dropdown mapDropdown;
        [SerializeField] private List<SceneAsset> availableMaps;
        [SerializeField] private Slider gameDurationSlider;
        [SerializeField] private Slider gameScoreSlider;
        [SerializeField] private TMP_Text gameDurationText;
        [SerializeField] private TMP_Text gameScoreText;

        private void Awake()
        {
            gameModeDropdown.options.Clear();
            foreach (var mode in System.Enum.GetValues(typeof(GameMode)))
            {
                gameModeDropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
            }
            mapDropdown.options.Clear();
            foreach (var scene in availableMaps)
            {
                mapDropdown.options.Add(new TMP_Dropdown.OptionData(scene.name));
            }
        }
        
        private void Start()
        {
            gameModeDropdown.value = (int) GlobalSettings.Instance.CurrentGameMode;
            mapDropdown.value = GlobalSettings.Instance.LevelSelected;
            gameDurationSlider.value = GlobalSettings.Instance.MaxTime;
            gameScoreSlider.value = GlobalSettings.Instance.ScoreGoal;
            UpdateGameDurationText();
            UpdateGameScoreText();
        }

        public void UpdateGameDurationText()
        {
            gameDurationText.text = gameDurationSlider.value.ToString(CultureInfo.InvariantCulture);
        }
        
        public void UpdateGameScoreText()
        {
            gameScoreText.text = gameScoreSlider.value.ToString(CultureInfo.InvariantCulture);
        }

        public void StartGame()
        {
            GlobalSettings.Instance.CurrentGameMode = (GameMode) gameModeDropdown.value;
            GlobalSettings.Instance.LevelSelected = mapDropdown.value;
            GlobalSettings.Instance.MaxTime = (int) gameDurationSlider.value;
            GlobalSettings.Instance.ScoreGoal = (int) gameScoreSlider.value;
            SceneManager.LoadScene(availableMaps[GlobalSettings.Instance.LevelSelected].name);
        }
    } 
}
