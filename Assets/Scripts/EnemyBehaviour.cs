using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] private float initialVerticalSpeed;
	[SerializeField] private float horizontalSpeedAmplitude;
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
		rBody.velocity = new Vector2(Random.Range(-horizontalSpeedAmplitude, horizontalSpeedAmplitude), initialVerticalSpeed);
	}

	public void TakeHit(Vector3 hitPosition)
	{
		Life -= 10;
		Vector3 impulseDirection = this.transform.position - hitPosition;
		rBody.AddForce(impulseForce * impulseDirection , ForceMode2D.Impulse);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// Debug.Log("Collision");
		if(other.gameObject.CompareTag("Bullet"))
		{
			TakeHit(other.transform.position);
			Destroy(other.gameObject);
		}
	}
}
