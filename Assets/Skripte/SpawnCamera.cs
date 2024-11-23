using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCamera : MonoBehaviour
{
    public Transform spawnPoint; // To je tvoj spawn point

    void Start()
    {
        // Preveri, ali je spawnPoint dodeljen
        if (spawnPoint != null)
        {
            // Premakni First Person Controller na spawn point
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation; // če želiš tudi obrniti kamero
        }
    }
}