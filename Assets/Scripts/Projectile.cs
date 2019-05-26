using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Damage caused in enemies by the projectile")]
    [SerializeField] private int damage;

    /// <summary>
    /// Destroys the projectile if it gets out of the game area
    /// </summary>
    /// <param name="other">Colldier from where the projectile is getting out</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GameArea"))
            Destroy(this.gameObject);
    }

    /// <summary>
    /// Does damage at enemy and destroys itself if it hits an enemy
    /// </summary>
    /// <param name="other">Collider of the object that the projectile is colliding with</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Life -= damage;
            Destroy(this.gameObject);
        }
    }
}