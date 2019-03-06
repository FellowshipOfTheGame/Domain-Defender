using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour 
{
	[Tooltip("Tag of the objects that will be destroyed by killzone")]
	[SerializeField] private string objectTag;

	// Destroys all objects with specified tag that collides with the killzone	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag(objectTag))
		{
			Destroy(other.gameObject);
		}
	}
}
