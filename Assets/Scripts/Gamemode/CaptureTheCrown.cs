using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheCrown : GameModeBase
{
    private GameObject crownPrefab;
    private Crown crownObject;
    private PlayerStatistics currentOwner;
    private float currentTimer;
    public CaptureTheCrown(float timerGame, int scoreGoal, GameObject crownPrefab) : base(timerGame, scoreGoal)
    {
        this.crownPrefab = crownPrefab;
    }

    public override void StartGame()
    {
        base.StartGame();
        GameObject gameObjectCrown = Object.Instantiate(crownPrefab);
        CrownSpawnerBehavior[] spawners = Object.FindObjectsOfType<CrownSpawnerBehavior>();
        int randomSpawner = Random.Range(0, spawners.Length);
        CrownSpawnerBehavior spawner = spawners[randomSpawner];
        gameObjectCrown.transform.position = spawner.transform.position;
        crownObject = gameObjectCrown.GetComponent<Crown>();
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
        if (crownObject.CurrentOwner)
        {
            Debug.Log("Adding score to crown");
            if(!currentOwner)
            {
                Debug.Log($"A new owner has been added to crown");
                currentOwner = crownObject.CurrentOwner.GetComponent<PlayerStatistics>();
                currentTimer = currentOwner.Score;
            }
            else
            {
                currentTimer += Time.deltaTime;
                if (currentTimer > 1f)
                {
                    currentTimer = 0f;
                    ++currentOwner.Score;
                }   
            }
        }
        else
        {
            currentOwner = null;
            currentTimer = 0f;
        }
    }
}
