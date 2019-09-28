using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable 
{
    [SerializeField] AudioClip PowerSound;
    /// <summary>
    /// Increses the number of coins
    /// </summary>
    /// <param name="other"></param>
    protected override void Effect(Collider2D other)
    {
        GameManager.instance.GetComponent<AudioSource>().PlayOneShot(PowerSound);
        ScoreBoard.instance.Coins++;
        Destroy(this.gameObject);
    }
}
