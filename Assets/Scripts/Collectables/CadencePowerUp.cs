using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadencePowerUp : Collectable
{
    /// <summary>
    /// Activates the cadence power up on player
    /// </summary>
    /// <param name="player"></param>
    protected override void Effect(Collider2D player)
    {
        player.GetComponent<PowerUps>().CadencePowerUp();
    }
}