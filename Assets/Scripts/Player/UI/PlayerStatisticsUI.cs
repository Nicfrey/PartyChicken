using System;
using Settings;
using TMPro;
using UnityEngine;

public class PlayerStatisticsUI : MonoBehaviour
{
    [SerializeField]
    private PlayerStatistics playerStatistics;
    [SerializeField]
    private ProgressBarBehavior progressBar;

    [SerializeField] 
    private TMP_Text killsText;
    
    [SerializeField]
    private TMP_Text deathsText;
    
    [SerializeField]
    private TMP_Text scoreText;
    
    private void Update()
    {
        progressBar.SetPercentage(playerStatistics.Score / (float) GlobalSettings.Instance.ScoreGoal);
        killsText.text = playerStatistics.Kills.ToString();
        deathsText.text = playerStatistics.Deaths.ToString();
        scoreText.text = playerStatistics.Score + "/" + GlobalSettings.Instance.ScoreGoal;
    }
}
