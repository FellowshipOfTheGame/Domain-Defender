using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] private float speed;
	public float Speed {get {return speed;}}
	[SerializeField] private float life;
	[SerializeField] private Vector2 impulseForce;
	
	private Rigidbody2D rBody;
	
	public float Life 
	{
		get 
		{
			return life;
		}
		set
		{
			life = value;

			if (value <= 0)
			{
				ScoreBoard.instance.OnEnemyDeath();
				Destroy(this.gameObject);
			}
		}
	}

	// Use this for initialization
	private void Start ()
	{
		rBody = GetComponent<Rigidbody2D>();
		GetComponent<Rigidbody2D>().velocity = Vector2.down * Speed;
	}

	public void TakeHit(Vector3 hitPosition)
	{
		Life -= 10;
		Vector3 impulseDirection = this.transform.position - hitPosition;
		rBody.AddForce(impulseForce * impulseDirection , ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Debug.Log("Collision");
		if(other.tag == "Bullet")
		{
			TakeHit(other.transform.position);
			Destroy(other.gameObject);
		}
	}
}
