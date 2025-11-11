using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillZone : MonoBehaviour
{
    public List<PlayerStatistics> playersInZone = new();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Target>().onDeath.AddListener(RemovePlayer);
        }
        playersInZone.Add(other.GetComponent<PlayerStatistics>());
    }

    private void RemovePlayer(PlayerStatistics player)
    {
        playersInZone.Remove(player);
        player.GetComponent<Target>().onDeath.RemoveListener(RemovePlayer);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RemovePlayer(other.GetComponent<PlayerStatistics>());
        }
    }
}
