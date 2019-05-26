using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Tooltip("Spawn distance from the center")]
    [SerializeField] float spawnDistance;
    [Tooltip("All enemies prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    private Vector3[] spawns = new Vector3[6];
    private Vector3 center = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Sets all the six spawns
    /// </summary>
    private void Start()
    {
        for (int i = 0, angle = 0; i < 6; i++, angle += 60)
        {
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
            spawns[i] = direction * spawnDistance;
        }
    }
}