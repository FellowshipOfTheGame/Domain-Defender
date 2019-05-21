using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private Vector3 mousePosition;
	private bool shooting = false;
	[Header("Balancement variables")]
	[Tooltip("Wait time for player to shoot again")]
	[SerializeField] private float shootingCooldown;
	[Tooltip("Player max speed to follow mouse position")]
	[SerializeField] private float speed;

	[Tooltip("Distance from center to screen borders. Used to limit player's movement")]
	[SerializeField] private float movementRange;
	[Space(5)]
	[Header("Object References")]
	[Tooltip("Proectile GameObject shot by player")]
	[SerializeField] private GameObject projectile;
	[Tooltip("Reference to Main Camera")]
	[SerializeField] private Camera mainCamera;

	
	// Update is called once per frame
	void FixedUpdate () 
	{
		// Player movement
		// Gets mouse position
		mousePosition = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, this.transform.position.y);

		// Limits movement to screen
		if (mousePosition.x > movementRange)
		{
			mousePosition = new Vector2 (movementRange, mousePosition.y);
		}
		else if (mousePosition.x < -movementRange)
		{
			mousePosition = new Vector2 (-movementRange, mousePosition.y);
		}

		// Moves it
		this.transform.position = Vector2.MoveTowards(this.transform.position, mousePosition, speed);

		// Shooting
		if (!shooting && Input.GetMouseButton(0))
		{
			StartCoroutine("ShootingCooldown");
			Instantiate(projectile, this.transform.position + Vector3.up, Quaternion.identity);
		}

	}

	// Wait time to shoot again
	IEnumerator ShootingCooldown() {
		shooting = true;
		yield return new WaitForSeconds(shootingCooldown);
		shooting = false;
	}
}