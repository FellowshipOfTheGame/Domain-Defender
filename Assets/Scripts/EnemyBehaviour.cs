using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] private float initialVerticalSpeed;
	[SerializeField] private float horizontalSpeedAmplitude;
	// public float Speed {get {return speed;}}

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-horizontalSpeedAmplitude, horizontalSpeedAmplitude), initialVerticalSpeed);
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Collision");
		if(other.tag == "Bullet")
		{
			ScoreBoard.instance.OnEnemyDeath();
			Destroy(this.gameObject);
			Destroy(other.gameObject);
		}
	}
}
