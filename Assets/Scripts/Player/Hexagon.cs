using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Hexagon : MonoBehaviour
{
    private bool hasShield;
    [SerializeField] GameObject shield;
    [SerializeField] LoadingPanel loadingPanel;
    [SerializeField] PowerUps powerUps;

    [SerializeField] private AudioSource ShieldDown;
    
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
        ShieldDown.Play();
        AnimManager.instance.hexagon.SetShield(false);
    }

    private void Reset()
    {
        Time.timeScale = 0f;
        int score = ScoreBoard.instance.Score;
        int coins = ScoreBoard.instance.Coins;
        bool noUpgradeCheat = powerUps.CheckUpgrades();

        if (noUpgradeCheat)
        {
            // If everything is ok
            if (score != -1  && coins != -1)
            {
                if (score != 0)
                {
                    WWWForm form = new WWWForm();
                    form.AddField("score", score.ToString());
                    form.AddField("money", coins.ToString());
                    // TODO: Tela de loading. Sugestão: usar classe LoadingPanel em um objeto de ui de painel.
                    // ! Veja o script stat upgrade menu para exemplos.

                    Time.timeScale = 0;
                    loadingPanel.StartLoading("Carregando...");
                    StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player", form, GoToUpgradesScene, SubmitScoreError));
                }
                else
                    GoToUpgradesScene(null);
            }
            else
                GoToUpgradesScene(null);
        }
        else
        {
            Debug.Log("Upgrade cheat detected");
            GoToUpgradesScene(null);
        }
    }

    private void SubmitScoreError(string errorMessage)
    {
        Debug.Log("Erro ao submeter score: " + errorMessage);
        // TODO: Lidar com erro. Sugestão: usar classe LoadingPanel em um objeto de ui de painel.

        Button.ButtonClickedEvent backToMenu = new Button.ButtonClickedEvent();
        backToMenu.AddListener(() => GameManager.instance.BackToMenu());
        loadingPanel.ShowError("Ops! Ocorreu um erro!", errorMessage, "Voltar", backToMenu);
    }

    private void GoToUpgradesScene(PlayerStats player)
    {
        Time.timeScale = 1;
        GameManager.instance.BackToMenu();
    }
}
