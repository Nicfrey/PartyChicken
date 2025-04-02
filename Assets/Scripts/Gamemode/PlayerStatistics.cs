using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{

    private Target target;
    public int Kills { get; private set; }
    
    public int Deaths { get; private set; }
    
    public int Score { get; set; }

    private void Start()
    {
        target = GetComponent<Target>();
        target.onDeath.AddListener(AddDeath);
    }
    

    private void AddKill()
    {
        Kills++;
    }

    private void AddDeath(PlayerStatistics playerShooting)
    {
        Deaths++;
        playerShooting?.AddKill();
    }

    public void ResetStats()
    {
        Score = 0;
        Kills = 0;
        Deaths = 0;
    }
}
