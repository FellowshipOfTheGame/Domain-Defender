using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Damage caused in enemies by the projectile")]
    [SerializeField] private int damage;
    [Tooltip("Number of hits on enemies before exploding")]
    [SerializeField] private int maxHits;
    [SerializeField] int hits = 0;

    // Sound
    [SerializeField] private AudioClip AcertaInimigo;
    [SerializeField] private AudioClip Pew;


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
            if (--hits == 0)
                this.GetComponent<Collider2D>().enabled = false;

            other.GetComponent<Enemy>().TakeDamage(damage);

            if(other.GetComponent<Enemy>().Life > 0)
                GameManager.instance.GetComponent<AudioSource>().PlayOneShot(AcertaInimigo);

            if (hits == 0)
                Destroy(this.gameObject);

        }
    }
    
    public void Initialize(int damage, int hits)
    {
        this.damage = damage;
        this.hits = hits;
    }

    public void Awake()
    {
        GameManager.instance.GetComponent<AudioSource>().PlayOneShot(Pew, 0.065f);
    }
}