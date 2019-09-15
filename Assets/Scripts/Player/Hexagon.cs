using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hexagon : MonoBehaviour
{
    private bool hasShield;
    [SerializeField] GameObject shield;
    
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
                DeactivateShield();
                Destroy(other.gameObject);
            }
            else
            {
                //SceneManager.LoadScene(0);
                AnimManager.instance.GameOver();
                Invoke("Reset", 1.3f);
            }

        }
    }

    /// <summary>
    /// Activates shield
    /// </summary>
    public void ActivateShield()
    {
        hasShield = true;
        shield.SetActive(true);
    }

    /// <summary>
    /// Deactivates shield
    /// </summary>
    private void DeactivateShield()
    {
        hasShield = false;
        shield.SetActive(false);
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
