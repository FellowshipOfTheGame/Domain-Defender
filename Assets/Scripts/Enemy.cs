using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Enemy's life")]
    [SerializeField] private int life;
    [Tooltip("Enemy's speed")]
    [SerializeField] private float speed;
    [Tooltip("Indicates if the enemy splits or not when its life reaches 0")] 
    [SerializeField] private bool split;

    [HideInInspectorIfNot(nameof(split))]
    [Tooltip("Prefab of the enemy spawned after split")]
    [SerializeField] private GameObject splitResultEnemy;

    [HideInInspectorIfNot(nameof(split))]
    [Tooltip("This distance will be summed to the current distance from center to calculate the distance form center of the enemies instantiated on split")]
    [SerializeField] private float knockBackDistance;

    private Vector3 center = new Vector3(0f, 0f, 0f);


    public int Life
    {
        get { return life; }

        // Checks if the enemy died after setting the new life value
        set
        {
            life = value;

            // If the enemy died, tries to split and 
            if (life <= 0)
            {
                if (split)
                    Split();
                else
                    ScoreBoard.instance.OnEnemyDeath();

                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Moves the enemy in direction of the center
    /// </summary>
    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, center, speed);
    }

    /// <summary>
    /// Detects game over condition (if an enemy hits the player)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Game Over");
    }

    /// <summary>
    /// Splits in two other enemies, that will spawn in the adjacent routes, a litte further from the center than this enemy is
    /// </summary>
    private void Split()
    {
        // Gets data from this enemy position and rotation in relation to the center
        Vector3 enemyDirection = this.transform.position - center;
        float distanceFromCenter = enemyDirection.magnitude;
        float angle = -Vector3.Angle(enemyDirection, Vector3.up);
        
        // Calculates the first new enemy spawn position and instantiates it
        Vector3 newEnemyDirection = (Quaternion.Euler(0f, 0f, angle + 60f) * Vector3.up).normalized;
        Vector3 spawnPosition = newEnemyDirection * (distanceFromCenter + knockBackDistance);
        Instantiate(splitResultEnemy, spawnPosition, Quaternion.identity);

        // Does the same for the second new enemy
        newEnemyDirection = (Quaternion.Euler(0f, 0f, angle - 60f) * Vector3.up).normalized;
        spawnPosition = newEnemyDirection * (distanceFromCenter + knockBackDistance);
        Instantiate(splitResultEnemy, spawnPosition, Quaternion.identity);
    }
}