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
        AnimManager.instance.hexagon.SetShield(true);
    }

    /// <summary>
    /// Deactivates shield
    /// </summary>
    public void DeactivateShield()
    {
        hasShield = false;
        AnimManager.instance.hexagon.SetShield(false);
    }

    private void Reset()
    {
        Time.timeScale = 0f;
        int score = ScoreBoard.instance.Score;
        int coins = ScoreBoard.instance.Coins;

        // If everything is ok
        if (score != -1  && coins != -1)
        {
            WWWForm form = new WWWForm();
            form.AddField("score", score.ToString());
            form.AddField("money", coins.ToString());
            StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player", form, GoToUpgradesScene));
        }
    }

    private void GoToUpgradesScene(PlayerStats player)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
