using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeOnHexagon;
    [SerializeField] float blinkingTIme;
    private Vector3 center = Vector3.zero;
    private bool moving = true;

    /// <summary>
    /// Moves the collectable to the center
    /// </summary>
    private void Update()
    {
        if (moving && !Pause.instance.paused)
            this.transform.position = Vector3.MoveTowards(this.transform.position, center, speed);
    }

    /// <summary>
    /// Stops the collectables when it reaches the collectable's area, and starts its effect when the player picks it up
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable'sArea"))
        {
            moving = false;
            StartCoroutine(SelfDestruct());
        }
        else if (other.CompareTag("Player"))
            Effect(other);
    }

    private IEnumerator SelfDestruct() 
    {
        yield return new WaitForSeconds(timeOnHexagon - blinkingTIme);
        //TODO start blinking
        yield return new WaitForSeconds(blinkingTIme);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Effect of the collectable. Runs when the player gets it.
    /// </summary>
    /// <param name="other">Player</param>
    protected abstract void Effect(Collider2D other);
}
