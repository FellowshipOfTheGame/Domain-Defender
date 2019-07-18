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
    [Tooltip("Enemy's speed while going to its lane")]
    [SerializeField] private float repositioningSpeed;
    [Tooltip("Nearest hexagon side")]
    [SerializeField] private int lane;
    [Tooltip("Indicates if the enemy splits or not when its life reaches 0")] 
    [SerializeField] private bool split;

    [HideInInspectorIfNot(nameof(split))]
    [Tooltip("Prefab of the enemy spawned after split")]
    [SerializeField] private GameObject splitResultEnemy;

    [HideInInspectorIfNot(nameof(split))]
    [Tooltip("This distance will be summed to the current distance from center to calculate the distance form center of the enemies instantiated on split")]
    [SerializeField] private float knockBackDistance;
    [HideInInspectorIfNot(nameof(split))]
    [Tooltip("If selected, it will split to all lanes on its death")]
    [SerializeField] private bool trojanHorse;

    private Vector3 center = Vector3.zero;
    private bool movingToLane = false;
    private Vector3 startPosition;

    private Collider2D col;


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
    /// Moves the enemy in direction of the center, or to its start position
    /// </summary>
    private void FixedUpdate()
    {
        if (movingToLane)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition, repositioningSpeed);
            if ((this.transform.position - startPosition).magnitude < 0.1f)
            {
                movingToLane = false;
                col.enabled = true;
            }

        }
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, center, speed);
    }


    public void MoveToLane(int lane, Vector3 position)
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        
        Lane = lane;
        startPosition = position;
        movingToLane = true;

    }

    /// <summary>
    /// Splits in two other enemies, that will spawn in the adjacent routes, a litte further from the center than this enemy is
    /// </summary>
    private void Split()
    {
        // Calculates the distance from the center
        float distanceFromCenter = (this.transform.position - center).magnitude;

        if (!trojanHorse)
        {
            // Instantiates one enemy left
            int newEnemyLane = Mod(lane + 1, 6);
            Vector3 newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
            newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
            GameObject instance = Instantiate(splitResultEnemy, this.transform.position, Quaternion.identity);
            instance.GetComponent<Enemy>().MoveToLane(newEnemyLane, newEnemySpawn);

            // Instantiates one enemy right
            newEnemyLane = Mod(lane - 1, 6);
            newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
            newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
            instance = Instantiate(splitResultEnemy, this.transform.position, Quaternion.identity);
            instance.GetComponent<Enemy>().MoveToLane(newEnemyLane, newEnemySpawn);
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (i != lane)
                {
                    int newEnemyLane = i;
                    Vector3 newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
                    newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
                    GameObject instance = Instantiate(splitResultEnemy, this.transform.position, Quaternion.identity);
                    instance.GetComponent<Enemy>().MoveToLane(newEnemyLane, newEnemySpawn);
                }
            }
        }
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