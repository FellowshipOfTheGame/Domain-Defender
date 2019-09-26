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
        public TextMeshProUGUI highscore;
        public Button upgradeButton;
    }

    [SerializeField] UIReferences uiReferences;

    [SerializeField] GameObject selectionIcon;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject defaultSelected;
    public static Upgrade[] upgradeableStats;
    StatType selectedStatType;

    private void Start()
    {
        StartCoroutine(NetworkManager.GetRequest<PlayerStats>("/player", LoadPlayerInfo));
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
            int level = playerStats[selectedStatType];

            uiReferences.cost.text = selectedStat.cost[level].ToString();
            uiReferences.currentValue.text = selectedStat.value[level].ToString();
            uiReferences.nextValue.text = selectedStat.value[level+1].ToString();
            uiReferences.coins.text = playerStats.money.ToString();
            uiReferences.highscore.text = playerStats.highScore.ToString();

            if(playerStats.money < selectedStat.cost[level])
                uiReferences.upgradeButton.enabled = false;
        }
    }
}
