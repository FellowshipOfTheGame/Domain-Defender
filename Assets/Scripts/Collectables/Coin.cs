using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable 
{
    /// <summary>
    /// Increses the number of coins
    /// </summary>
    /// <param name="other"></param>
    protected override void Effect(Collider2D other)
    {
        //TODO GameManager.instance.coins++;
    }
}
