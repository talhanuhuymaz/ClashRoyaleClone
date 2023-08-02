using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPos;

    void Start()
    {
        InvokeRepeating("Spawn", 0, Random.Range(5f, 15f));
    }

    
   private void Spawn()
    {
        Instantiate(enemy, spawnPos.position, Quaternion.identity);
    }
}
