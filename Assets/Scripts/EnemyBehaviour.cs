using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] private float speed;
	public float Speed {get {return speed;}}

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.down * Speed;
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
