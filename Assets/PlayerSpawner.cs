using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab FirstPersonController
    public Transform spawnPoint; // Spawn point

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Ustvari igralca na spawn pointu
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}