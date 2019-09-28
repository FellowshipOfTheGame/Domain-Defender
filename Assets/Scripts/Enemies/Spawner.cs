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
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int waveSize;
    private int spawnedEnemiesOnThisWave = 0;
    [SerializeField] float waveIncreasePercentage;
    [SerializeField] float timeBetweenSpawnsDecreasePercentage;
    public float dps;


    public float coinDropProbability;
    public float powerUpDropProbability;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject[] powerUpsPrefabs;

    public AnimationCurve curve;
    public AnimationCurve enemyLifeCurve;

    public Vector3[] Spawns => spawns;
    Enemies spawnedEnemies;

    private struct Enemies
    {
        public int basic;
        public int splittable;
        public int trojanHorse;
        public int sum;
    }


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
    }

    public void Initialize(float dps)
    {
        this.dps = dps;
        StartCoroutine(StartSpawner());       
    }

    public void LevelUp()
    {
        timeBetweenSpawns *= (1 - timeBetweenSpawnsDecreasePercentage);
        waveSize = (int)Mathf.Ceil(waveSize * 1f + waveIncreasePercentage);
        spawnedEnemiesOnThisWave = 0;
    }

    private IEnumerator StartSpawner()
    {
        while (true)
        {
            int spawnIndex = Random.Range(0, 6);
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            // Vector2 direction = (Vector2)(spawns[spawnIndex]).normalized;
            // Quaternion rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, direction));
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawns[spawnIndex], Quaternion.identity);
            enemy.transform.up = spawns[spawnIndex];
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.Lane = spawnIndex;
            enemyScript.BaseLife = (int)(dps * enemyLifeCurve.Evaluate(Time.timeSinceLevelLoad));
            Debug.Log("Enemy life: " + enemyScript.Life);

            // if splittable
            if (enemyIndex == 1)
                enemyScript.BaseLife *= 2;
            // if trojan horse
            if (enemyIndex == 2)
                enemyScript.BaseLife *= 3;

            

            Count(enemyIndex);

            // yield return new WaitForSeconds(timeBetweenSpawns);
            Debug.Log("Current time: " + Time.timeSinceLevelLoad);
            Debug.Log("Time until next spawn: " + curve.Evaluate(Time.timeSinceLevelLoad));
            yield return new WaitForSeconds(curve.Evaluate(Time.timeSinceLevelLoad));
        }
    }

    private void Count(int enemyIndex)
    {
        switch (enemyIndex)
        {
            case 0:
                spawnedEnemies.basic++;
                break;

            case 1:
                spawnedEnemies.splittable++;
                break;

            case 2:
                spawnedEnemies.trojanHorse++;
                break;
        }
        spawnedEnemies.sum++;
        spawnedEnemiesOnThisWave++;

        if (spawnedEnemiesOnThisWave == waveSize)
            LevelUp();
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
        int powerUpIndex = Random.Range(0, powerUpsPrefabs.Length);
        Instantiate(powerUpsPrefabs[powerUpIndex], position, rotation);
    }
}