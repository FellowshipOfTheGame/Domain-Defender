using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    [Tooltip("Spawn distance from the center")]
    [SerializeField] float spawnDistance;
    [Tooltip("All enemies prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    private Vector3[] spawns = new Vector3[6];
    private Vector3 center = new Vector3(0f, 0f, 0f);
    private float timeBetweenSpawns = 3f;

    public float coinDropProbability;
    public float powerUpDropProbability;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject[] powerUpsPrefabs;

    public Vector3[] Spawns => spawns;

    /// <summary>
    /// Checks singleton conditions, sets all the six spawns, and then starts the spawner
    /// </summary>
    private void Start()
    {
		// Singleton
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy(this);

        // Sets spawns
        for (int i = 0, angle = 0; i < 6; i++, angle += 60)
        {
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
            spawns[i] = direction * spawnDistance;
        }

        StartCoroutine(StartSpawner());
    }

    public void LevelUp()
    {
        timeBetweenSpawns -= 0.1f;
    }

    private IEnumerator StartSpawner()
    {
        while (true)
        {
            int spawnIndex = Random.Range(0, 6);
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawns[spawnIndex], Quaternion.identity);
            enemy.GetComponent<Enemy>().Lane = spawnIndex;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    /// <summary>
    /// Drops a coin with position and rotation of the destroyed enemy
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void DropCoin(Vector3 position, Quaternion rotation)
    {
        Instantiate(coinPrefab, position, rotation);
    }

    /// <summary>
    /// Drops a power up with position and rotation of the destroyed enemy
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void DropPowerUp(Vector3 position, Quaternion rotation)
    {
        int powerUpIndex = Random.Range(0, powerUpsPrefabs.Length - 1);
        Instantiate(powerUpsPrefabs[powerUpIndex], position, rotation);
    }
}