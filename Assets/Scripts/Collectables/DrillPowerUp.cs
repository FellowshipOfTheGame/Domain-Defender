using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillPowerUp : Collectable
{
    [SerializeField] AudioClip PowerSound;
    /// <summary>
    /// Activates the drill power up on player
    /// </summary>
    /// <param name="player"></param>
    protected override void Effect(Collider2D player)
    {
        GameManager.instance.GetComponent<AudioSource>().PlayOneShot(PowerSound);
        player.GetComponent<PowerUps>().DrillPowerUp();
        Destroy(this.gameObject);
    }
}