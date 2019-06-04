using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Tooltip("Nearest hexagon side")]
    [SerializeField] private int lane;
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

    public int Lane
    {
        get { return lane; }

        // Sets only if the new value is between 0 and 5 (inclusive)
        set 
        {
            if (value >= 0 && value < 6)
                lane = value;
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
    /// Splits in two other enemies, that will spawn in the adjacent routes, a litte further from the center than this enemy is
    /// </summary>
    private void Split()
    {
    /*
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
    */
        
        // Calculates the distance
        float distanceFromCenter = (this.transform.position - center).magnitude;

        // Instantiates one enemy left
        int newEnemyLane = Mod(lane + 1, 6);
        Vector3 newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
        newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
        GameObject instance = Instantiate(splitResultEnemy, newEnemySpawn, Quaternion.identity);
        instance.GetComponent<Enemy>().Lane = newEnemyLane;

        // Instantiates one enemy right
        newEnemyLane = Mod(lane - 1, 6);
        newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
        newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
        instance = Instantiate(splitResultEnemy, newEnemySpawn, Quaternion.identity);
        instance.GetComponent<Enemy>().Lane = newEnemyLane;
    }

    /// <summary>
    /// Calculates modulus
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <returns>Result of the operation</returns>
    private int Mod(int dividend, int divisor)
    {
        int result = dividend % divisor;
        if (result < 0)
            result += divisor;
        return result;
    }
}