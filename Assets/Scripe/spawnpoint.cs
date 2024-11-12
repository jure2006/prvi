using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject objectToSpawn;  // Prefab objekta, ki ga želiš spawnati

    // Funkcija za spawnanje objekta
    public void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab for spawning is not assigned.");
        }
    }

    // Spawn ob začetku igre (neobvezno)
    void Start()
    {
        SpawnObject();
    }
}