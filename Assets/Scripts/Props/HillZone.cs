using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillZone : MonoBehaviour
{
    private List<PlayerStatistics> _playersInZone = new();
    public List<PlayerStatistics> PlayersInZone => _playersInZone;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        _meshRenderer.enabled = isActive;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Adding player {LayerMask.LayerToName(other.gameObject.layer)} from hill zone");
            _playersInZone.Add(other.GetComponent<PlayerStatistics>());
        }
    }

    private void RemovePlayer(PlayerStatistics player)
    {
        Debug.Log($"Removing player {LayerMask.LayerToName(player.gameObject.layer)} from hill zone");
        _playersInZone.Remove(player);
    }

    private void Update()
    {
        List<int> playersToRemove = new();
        for (int i = 0; i < _playersInZone.Count; i++)
        {
            if(_playersInZone[i].GetComponent<Target>().IsDead())
            {
                playersToRemove.Add(i);
            }
        }
        for (int i = playersToRemove.Count - 1; i >= 0; i--)
        {
            _playersInZone.RemoveAt(playersToRemove[i]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RemovePlayer(other.GetComponent<PlayerStatistics>());
        }
    }

    public bool IsPlayerInHill(PlayerStatistics player)
    {
        return _playersInZone.Contains(player);
    }

    public bool HasOnlyOnePlayerInHill()
    {
        return _playersInZone.Count == 1;
    }
}
