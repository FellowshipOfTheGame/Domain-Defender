using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour 
{
    [Tooltip("Projectile speed")]
    [SerializeField] private float speed;
    [SerializeField] private float distanceBetweenProjectiles;
    [SerializeField] private Transform projectileSpawn;
    
    [Space(5)]
    [Header("These variabes are initialized by PowerUps")]
    [Tooltip("Projectile prefab")]
    public GameObject projectile;
    [Tooltip("Wait time between shots")]
    public float cooldown;
    public int numberOfBullets;
    public int numberOfHits;
    public int damage;
    public bool drillPowerUp = false;

    public int type;
    private bool canAttack = true;


    /// <summary>
    /// Checks if the attack button is pressed and attacks if it can attack
    /// </summary>
    private void Update()
    {
        if (canAttack && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
        {
            StartCoroutine(Attack(GenerateSpawns(numberOfBullets)));
        }
    }

    /// <summary>
    /// Instantiate a projectile and sets its velocity (in the direction it is facing)
    /// Disables attack for "cooldown" seconds
    /// </summary>
    /// <returns>Cooldown time</returns>
    private IEnumerator Attack(Vector3[] spawns)
    {
        // Disables attack
        canAttack = false;

        // Converts rotation to direction vector, and then calculates the spawn position based on direction and offset
        // Vector3 direction = (this.transform.rotation * Vector3.up).normalized;
        // Vector3 spawnPosition = this.transform.position + direction * offsetDistance;

        // Instantiates the projectile and sets its velocity
        // GameObject instance = Instantiate(projectile, spawnPosition, this.transform.rotation);
        for (int i = 0; i < spawns.Length; i++)
        {
            GameObject instance = Instantiate(projectile, spawns[i], this.transform.rotation);
            instance.GetComponent<Rigidbody2D>().velocity = this.transform.up * speed;
            instance.GetComponent<BulletAnimHandle>().Initialize();
            if (drillPowerUp)
                instance.GetComponent<Projectile>().Initialize(damage, numberOfHits);
            else
                instance.GetComponent<Projectile>().Initialize(damage, 1);

        }

        // Waits "cooldown" seconds to enable player to shoot again 
        yield return new WaitForSecondsRealtime(cooldown);
        canAttack = true;
    }

    Vector3[] GenerateSpawns(int numberOfProjectiles)
    {
        Vector3[] spawns = new Vector3[numberOfProjectiles];

        if (numberOfProjectiles % 2 == 0)
        {
            float distance = distanceBetweenProjectiles;
            spawns[0] = projectileSpawn.position + (distance/2 * projectileSpawn.right);
            spawns[1] = projectileSpawn.position + (distance/2 * -projectileSpawn.right);

            for (int i = 2; i < numberOfProjectiles; i++)
            {
                spawns[i] = projectileSpawn.position + ((Mathf.Abs(distance/2) + Mathf.Ceil((float)i/2) * distance) * projectileSpawn.right);
                distance *= -1;
            }
        }
        else
        {
            spawns[0] = projectileSpawn.position;
            float distance = distanceBetweenProjectiles;
            for (int i = 1; i < numberOfProjectiles; i++)
            {
                spawns[i] = projectileSpawn.position + (Mathf.Ceil((float)i/2) * distance * projectileSpawn.right);
                distance *= -1;
            }
        }

        return spawns;
    }

}