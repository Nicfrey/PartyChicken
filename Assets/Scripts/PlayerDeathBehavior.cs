using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDeathBehavior : MonoBehaviour
{
    private Target target;
    private bool isRespawning = false;

    [SerializeField]
    private float respawnTime = 3.0f;
    private float respawnTimer;

    [SerializeField]
    private TextMeshProUGUI respawnTimerText;
    [SerializeField]
    private GameObject respawnTimerUI;

    void Start()
    {
        respawnTimerUI.SetActive(false);
        target = GetComponent<Target>();
        target.onDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        if (!isRespawning)
        {
            isRespawning = true;
            respawnTimerUI.SetActive(true);
        }
    }

    private void HandleTimerRespawn()
    {
        respawnTimer += Time.deltaTime;
        respawnTimerText.text = ((int)(respawnTime - respawnTimer)).ToString();
        if (respawnTimer >= respawnTime)
        {
            isRespawning = false;
            respawnTimer = 0;
            respawnTimerUI.SetActive(false);
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForEndOfFrame();
        
        SpawnPointBehavior[] spawnPoints = FindObjectsOfType<SpawnPointBehavior>();
        bool foundSpawnPoint = false;
        while (!foundSpawnPoint)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            if (spawnPoints[randomIndex].GetComponent<SpawnPointBehavior>().IsAvailable())
            {
                GetComponent<PlayerMovement>().SetPlayerPositionAndRotation(spawnPoints[randomIndex].transform.position,Quaternion.identity);
                spawnPoints[randomIndex].GetComponent<SpawnPointBehavior>().SetUnavailable();
                foundSpawnPoint = true;
            }
        }
        target.Revive();
    }

    void Update()
    {
        if (isRespawning)
        {
            HandleTimerRespawn();
        }
    }
}
