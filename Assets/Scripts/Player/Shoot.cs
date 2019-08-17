using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour 
{
    [Tooltip("Projectile speed")]
    [SerializeField] private float speed;
    [Tooltip("Distance between shot spawn and the center of the player")]
    [SerializeField] private float offsetDistance;
    
    [Space(5)]
    [Header("These variabes are initialized by PowerUps")]
    [Tooltip("Projectile prefab")]
    public GameObject projectile;
    [Tooltip("Wait time between shots")]
    public float cooldown;

    private bool canAttack = true;


    /// <summary>
    /// Checks if the attack button is pressed and attacks if it can attack
    /// </summary>
    private void Update()
    {
        if (canAttack && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
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