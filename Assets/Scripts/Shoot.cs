using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour 
{
    [Tooltip("Projectile prefab")]
    [SerializeField] private GameObject projectile;
    [Tooltip("Projectile speed")]
    [SerializeField] private float speed;
    [Tooltip("Wait time between shots")]
    [SerializeField] private float cooldown;
    [Tooltip("Distance between shot spawn and the center of the player")]
    [SerializeField] private float offsetDistance;
    private bool canAttack = true;

    /// <summary>
    /// Checks if the attack button was pressed
    /// </summary>
    private void Update()
    {
        if (canAttack && Input.GetKey(KeyCode.Space))
            StartCoroutine(Attack());
    }

    /// <summary>
    /// Instantiate a projectile and sets its velocity (in the direction it is facing)
    /// Disables attack for "cooldown" seconds
    /// </summary>
    /// <returns>Cooldown time</returns>
    private IEnumerator Attack()
    {
        // Disables attack
        canAttack = false;

        // Converts rotation to direction vector, and then calculates the spawn position based on direction and offset
        Vector3 direction = (this.transform.rotation * Vector3.up).normalized;
        Vector3 spawnPosition = this.transform.position + direction * offsetDistance;

        // Instantiates the projectile and sets its velocity
        GameObject instance = Instantiate(projectile, spawnPosition, this.transform.rotation);
        instance.GetComponent<Rigidbody2D>().velocity = direction * speed;

        // Waits "cooldown" seconds to enable player to shoot again 
        yield return new WaitForSecondsRealtime(cooldown);
        canAttack = true;
    }
}