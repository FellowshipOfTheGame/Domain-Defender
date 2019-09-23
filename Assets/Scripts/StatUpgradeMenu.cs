using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatUpgradeMenu : MonoBehaviour
{
    public static StatUpgradeMenu instance;

    private void Awake()
    {
        if(instance != this)
            instance = this;
        else if(instance != null)
            Destroy(this);   
    }

    [System.Serializable] struct UIReferences
    {
        public TextMeshProUGUI coins;
        public TextMeshProUGUI cost;
        public TextMeshProUGUI currentLevel;
        public TextMeshProUGUI nextLevel;
        public TextMeshProUGUI currentValue;
        public TextMeshProUGUI nextValue;
        public Button upgradeButton;
    }

    [SerializeField] UIReferences uiReferences;

    [SerializeField] GameObject selectionIcon;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject defaultSelected;
    Upgrade[] upgradeableStats;
    StatType selectedStatType;

    private void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", "XxDarkChickenxX");
        form.AddField("password", "galinha123");

        StartCoroutine(NetworkManager.PostRequest<Token>("/login", form, ValidateLogin));
    }

    private void ValidateLogin(Token token)
    {
        NetworkManager.token = token.token;
        playerStats = token.player;
        Debug.Log(token.token);

        // StartCoroutine(NetworkManager.GetRequest<PlayerStats>("/player", LoadPlayerInfo));
        StartCoroutine(NetworkManager.GetRequest<HighScores>("/highScores", LoadHighscores));
        StartCoroutine(NetworkManager.GetRequest<Upgrades>("/upgrade", LoadUpgradeInfo));
    }

    private void LoadPlayerInfo(PlayerStats stats)
    {
        playerStats = stats;
        UpdateUI();
    }

    private void LoadUpgradeInfo(Upgrades upgrades)
    {
        upgradeableStats = upgrades.upgrades;
        Select(defaultSelected.GetComponent<SelectableStatUpgrade>().statType, defaultSelected); 
    }

    private void LoadHighscores(HighScores upgrades)
    {
        // upgradeableStats = upgrades.upgrades;
        // Select(defaultSelected.GetComponent<SelectableStatUpgrade>().statType, defaultSelected); 
    }

    public void Select(StatType selectedStatType, GameObject selectedObject)
    {
        selectionIcon.transform.position = selectedObject.transform.position;
        this.selectedStatType = selectedStatType;
        
        UpdateUI();
    }

    public void Upgrade()
    {
        WWWForm form = new WWWForm();
        form.AddField("upgrade", ((int)selectedStatType).ToString());

        StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player/upgrade", form, FinishUpgrade));
    }

    private void FinishUpgrade(PlayerStats player)
    {
        playerStats = player;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(upgradeableStats != null && playerStats != null)
        {
            Upgrade selectedStat = upgradeableStats[(int)selectedStatType];
            Debug.Log((int)selectedStatType);
            int level = playerStats[selectedStatType];

            uiReferences.cost.text = selectedStat.cost[level].ToString();
            uiReferences.currentValue.text = selectedStat.value[level].ToString();
            uiReferences.nextValue.text = selectedStat.value[level+1].ToString();

            if(playerStats.money < selectedStat.cost[level])
                uiReferences.upgradeButton.enabled = false;
        }
    }
}
