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
    [Tooltip("Enemy's vertical speed while going to its lane")]
    [SerializeField] private float repositioningRadiusSpeed;
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
    [SerializeField] private float timeToKill;

    // Sounds
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioClip SplitSound;

    private Vector3 center = Vector3.zero;
    private bool movingToLane = false;
    private Vector3 startPosition;

    private Collider2D col;

    EnemyAnimHandle anim;


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
                {
                    GameManager.instance.GetComponent<AudioSource>().PlayOneShot(SplitSound);
                    StartCoroutine(Split());
                }
                else
                {
                    GameManager.instance.GetComponent<AudioSource>().PlayOneShot(DieSound);
                    Die();
                }

                ScoreBoard.instance.OnEnemyDeath();
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

    private void Start()
    {
        life = (int)Mathf.Ceil(Spawner.instance.dps * timeToKill);
        anim = GetComponent<EnemyAnimHandle>();
    }


    public void TakeDamage(int value){
        Life -= value;
        anim.GetDamage();
    }

    /// <summary>
    /// Moves the enemy in direction of the center, or to its start position
    /// </summary>
    private void FixedUpdate()
    {
        if (movingToLane)
            Reposition();
        // {
            // this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition, repositioningSpeed);
            // if ((this.transform.position - startPosition).magnitude < 0.1f)
            // {
            //     movingToLane = false;
            //     col.enabled = true;
            // }

        // }
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, center, speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Moves the enemy to its start postion when spawned
    /// </summary>
    private void Reposition()
    {
        // Circular movement
        this.transform.RotateAround(center, Vector3.forward, repositioningSpeed * Time.fixedDeltaTime);
        
        // Gradually increases radius
        Vector2 distance = (this.transform.position - center).normalized;
        Vector2 desiredPosition = distance * (startPosition - center).magnitude + (Vector2)center;
        this.transform.position = Vector2.MoveTowards(this.transform.position, desiredPosition, repositioningRadiusSpeed * Time.fixedDeltaTime);

        // Stops repositioning when it reaches the start position
        if ((this.transform.position - startPosition).magnitude < 0.1f)
            movingToLane = false;

    }

    /// <summary>
    /// Retruns the number turns needed for lane1 to reach lane2
    /// </summary>
    /// <param name="lane1"></param>
    /// <param name="lane2"></param>
    /// <returns>Number of turns needed</returns>
    private int DistanceBetweenLanes(int lane1, int lane2)
    {
        int diff = lane1 - lane2;
        if (Mathf.Abs(diff) > 3)
        {
            if (diff < 0)
                return 6  + diff;
            else 
                return -(6 - diff);
        }
        else
            return diff;
    }

    /// <summary>
    /// Sets the variables to make the enemy move to his lane after spawn
    /// </summary>
    /// <param name="lane"></param>
    /// <param name="position"></param>
    public void MoveToLane(int originLane, int destinyLane, Vector3 position)
    {
        Lane = destinyLane;
        startPosition = position;

        int laneDiff = DistanceBetweenLanes(originLane, destinyLane);
        if (laneDiff > 0)
            repositioningSpeed *= -1;

        movingToLane = true;

    }

    /// <summary>
    /// Splits in two other enemies, that will spawn in the adjacent routes, a litte further from the center than this enemy is
    /// </summary>
    private IEnumerator Split()
    {
        // Calculates the distance from the center
        float distanceFromCenter = (this.transform.position - center).magnitude;

        // Instantiates a enemy left
        int newEnemyLane = Mod(lane + 1, 6);
        Vector3 newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
        newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
        GameObject instance = Instantiate(splitResultEnemy, this.transform.position, this.transform.rotation);
        instance.GetComponent<Enemy>().MoveToLane(this.lane, newEnemyLane, newEnemySpawn);

        // Instantiates a enemy right
        newEnemyLane = Mod(lane - 1, 6);
        newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
        newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
        instance = Instantiate(splitResultEnemy, this.transform.position, this.transform.rotation);
        instance.GetComponent<Enemy>().MoveToLane(this.lane, newEnemyLane, newEnemySpawn);

        if (trojanHorse)
        {
            yield return new WaitForSeconds(0.1f);

            // Instantiates a enemy two lanes left
            newEnemyLane = Mod(lane + 2, 6);
            newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
            newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
            instance = Instantiate(splitResultEnemy, this.transform.position, this.transform.rotation);
            instance.GetComponent<Enemy>().MoveToLane(this.lane, newEnemyLane, newEnemySpawn);

            // Instantiates a enemy two lanes right
            newEnemyLane = Mod(lane - 2, 6);
            newEnemySpawn = Spawner.instance.Spawns[newEnemyLane];
            newEnemySpawn = (newEnemySpawn - center).normalized * (distanceFromCenter + knockBackDistance);
            instance = Instantiate(splitResultEnemy, this.transform.position, this.transform.rotation);
            instance.GetComponent<Enemy>().MoveToLane(this.lane, newEnemyLane, newEnemySpawn);
        }



        Die();
    }
    

    /// <summary>
    /// Destroys itself with a chance of dropping a power up. If the power up is not dropped,
    /// there is still a chance to drop a coin.
    /// </summary>
    private void Die()
    {
        bool powerUpDrop = Random.Range(0f, 1f) < Spawner.instance.powerUpDropProbability;
        if (powerUpDrop)
            Spawner.instance.DropPowerUp(this.transform.position, this.transform.rotation);
        else
        {
            bool coinDrop = Random.Range(0f, 1f) < Spawner.instance.coinDropProbability;
            if (coinDrop)
                Spawner.instance.DropCoin(this.transform.position, this.transform.rotation);
        }           

        anim.Explode();
        this.GetComponent<Collider2D>().enabled = false;
        speed = 0.0f;
        CancelInvoke();
        Invoke("Finish", 2.0f);
    }

    void Finish(){
        Destroy(this.gameObject);
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