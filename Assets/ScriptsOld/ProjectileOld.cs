using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOld : MonoBehaviour 
{
	private Rigidbody2D rBody;
	[Tooltip("Proejctile speed")]
	[SerializeField] private float speed;

	// Moves projectile
	void Start () 
	{
		rBody = GetComponent<Rigidbody2D>();
		rBody.velocity = new Vector2(0f, speed);
	}
}	