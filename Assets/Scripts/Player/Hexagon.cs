using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Security.Cryptography;


public class Hexagon : MonoBehaviour
{
    [SerializeField] bool hasShield;
    [SerializeField] GameObject shield;
    [SerializeField] LoadingPanel loadingPanel;
    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] PowerUps powerUps;
    private bool shieldDestroying = false;

    [SerializeField] private AudioSource ShieldDown;
    [SerializeField] private AudioClip DieSound;
    
    /// <summary>
    /// Detects game over condition (if an enemy hits the hexagon)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (shieldDestroying)
            {
                Destroy(other.gameObject);
            }
            else if (hasShield)
            {
                DeactivateShield();
                Destroy(other.gameObject);
            }
            else
            {
                //SceneManager.LoadScene(0);
                //GameManager.instance.GetComponent<AudioSource>().PlayOneShot(DieSound);
                AnimManager.instance.GameOver();
                Invoke("Reset", 1.3f);
            }

        }
    }

    string Hash()
    {
        byte[] bytes = Encoding.ASCII.GetBytes("oisemcomp" + ScoreBoard.instance.Score +
                                                             ScoreBoard.instance.Coins + powerUps.gamesPlayed);

        SHA1 sha = new SHA1CryptoServiceProvider(); 
        byte[] hashb = sha.ComputeHash(bytes);

        string hash = ByteArrayToString(hashb);

        Debug.Log("Hash = " + hash);
        return hash;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
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
        Invoke(nameof(NotInvincibleAnymore), 1f);
        shieldDestroying = true;
        hasShield = false;
        ShieldDown.Play();
        AnimManager.instance.hexagon.SetShield(false);
    }

    public void FadeOut()
    {
        hasShield = false;
        AnimManager.instance.hexagon.FadeOut();
    }

    void NotInvincibleAnymore()
    {
        shieldDestroying = false;
    }

    private void Reset()
    {
        Time.timeScale = 0f;
        int score = ScoreBoard.instance.Score;
        int coins = ScoreBoard.instance.Coins;
        bool noUpgradeCheat = powerUps.CheckUpgrades();
        Pause.instance.CanChangeState = false;

        Debug.Log(noUpgradeCheat + " && " + powerUps.noCheats);

        if (noUpgradeCheat && powerUps.noCheats)
        {
            // If everything is ok
            if (score != -1  && coins != -1)
            {
                if (score != 0)
                {
                    WWWForm form = new WWWForm();
                    Upgrades upgrades = new Upgrades();
                    upgrades.upgrades = StatUpgradeMenu.upgradeableStats;

                    form.AddField("score", score.ToString());
                    form.AddField("money", coins.ToString());
                    form.AddField("upgrades", JsonUtility.ToJson(upgrades));
                    form.AddField("levels", JsonUtility.ToJson(PowerUps.playerStats));
                    form.AddField("hash", Hash());

                    Time.timeScale = 0;
                    loadingPanel.StartLoading("Carregando...");
                    //StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player", form, ShowScore, SubmitScoreError));
                    ShowScore();
                }
                else
                {
                    loadingPanel.StartLoading("Carregando...");
                    ShowScore(null);
                }
            }
            else
            {
                Debug.Log("SENDING HACK TO SERVER");
    
                WWWForm form = new WWWForm();
                form.AddField("hacks", 1);
                
                Time.timeScale = 0;
                loadingPanel.StartLoading("Carregando...");
                GoToUpgradesScene();
                //StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player", form, GoToUpgradesScene, SubmitScoreError));
            }
        }
        else
        {
            Debug.Log("SENDING HACK TO SERVER");
            WWWForm form = new WWWForm();
            form.AddField("hacks", 1);

            Time.timeScale = 0;
            loadingPanel.StartLoading("Carregando...");

            GoToUpgradesScene();
            //StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player", form, GoToUpgradesScene, SubmitScoreError));
        }
    }

    private void SubmitScoreError(string errorMessage)
    {
        Debug.Log("Erro ao submeter score: " + errorMessage);

        Button.ButtonClickedEvent backToMenu = new Button.ButtonClickedEvent();
        backToMenu.AddListener(() => GameManager.instance.BackToMenu());
        loadingPanel.ShowError("Ops! Ocorreu um erro!", errorMessage, "Voltar", backToMenu);
    }

    private void ShowScore(PlayerStats player)
    {
        loadingPanel.StopLoading();
        scorePanel.SetActive(true);
        scoreText.text = "Pontuação:\n" + ScoreBoard.instance.Score.ToString();
    }

    private void ShowScore()
    {
        loadingPanel.StopLoading();
        scorePanel.SetActive(true);
        scoreText.text = "Pontuação:\n" + ScoreBoard.instance.Score.ToString();
    }

    public void GoToUpgradesScene(PlayerStats player)
    {
        Time.timeScale = 1;
        GameManager.instance.BackToMenu();
    }

    public void GoToUpgradesScene()
    {
        Time.timeScale = 1;
        GameManager.instance.BackToMenu();
    }
}
