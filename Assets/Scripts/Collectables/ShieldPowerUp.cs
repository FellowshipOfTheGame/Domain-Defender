using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : Collectable 
{
    /// <summary>
    /// Activates the shield power up on
    /// </summary>
    /// <param name="player"></param>
    protected override void Effect(Collider2D player)
    {
        player.GetComponent<PowerUps>().ShieldPowerUp();
        Destroy(this.gameObject);
    }
}
