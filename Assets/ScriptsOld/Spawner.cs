using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour {

	[SerializeField] private GameObject enemyPrefab;

	[Space(5)]

	[Header("Settings")]

	[Space(5)]

	[Tooltip("Radius of the spawner area (Check gizmos).")]
	[SerializeField] private float range;

	[Tooltip("Initial spawn time interval.")]
	[SerializeField] private float initialSpawnInterval;

	[Tooltip("Higher values mean the game gets harder faster.")]
	[SerializeField] private float difficulty;

	private Vector3 currentSpawnPos;
	
	private void Start()
	{
		StartCoroutine("StartSpawner");
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(this.transform.position - Vector3.left * range, "Limiter", false);
		Gizmos.DrawIcon(this.transform.position - Vector3.right * range, "Limiter", false);	
	}

	IEnumerator StartSpawner(/* Information on starting player stats */)
	{
		// Gets information on player stats to balance out starting hp
		while(true)
		{
			SpawnEnemy();
			Debug.Log(initialSpawnInterval);
			Debug.Log(ScoreBoard.instance.Score);
			Debug.Log(1 / (Mathf.Pow((ScoreBoard.instance.Score * difficulty), 2f) + 1/initialSpawnInterval));
			yield return new WaitForSeconds(1 / (Mathf.Pow((ScoreBoard.instance.Score * difficulty), 2f) + 1/initialSpawnInterval));
		}
	}

	private void SpawnEnemy(/* Enemy information */)
	{
		currentSpawnPos = new Vector3(Random.Range(-range, range ), 0, 0);
		Instantiate(enemyPrefab, currentSpawnPos, Quaternion.identity, this.transform);
	}

	// Temp, Debug.
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
			SpawnEnemy();
	}
}