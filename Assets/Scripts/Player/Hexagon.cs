using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hexagon : MonoBehaviour
{
    private bool hasShield;
    
    /// <summary>
    /// Detects game over condition (if an enemy hits the hexagon)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hasShield)
            {
                hasShield = false;
                Destroy(other.gameObject);
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    /// <summary>
    /// Activates shield
    /// </summary>
    public void ActivateShield()
    {
        hasShield = true;
        //TODO enable shield object
    }
}
