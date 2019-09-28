using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatUpgradeMenu : MonoBehaviour
{
    public static StatUpgradeMenu instance;
    [SerializeField] LoadingPanel loadingPanel;
    [SerializeField] GameObject rankingsTab;

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
    GameObject currentTab;

    private void OnEnable()
    {
        currentTab = this.gameObject;
        loadingPanel.StartLoading("Carregando...");
        StartCoroutine(NetworkManager.GetRequest<PlayerStats>("/player", LoadPlayerInfo, LoadError));
        StartCoroutine(NetworkManager.GetRequest<Upgrades>("/upgrade", LoadUpgradeInfo, LoadError));
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

    private void LoadError(string errorMessage)
    {
        Button.ButtonClickedEvent backToMenu = new Button.ButtonClickedEvent();
        backToMenu.AddListener(() => currentTab.SetActive(false));
        loadingPanel.ShowError("Ops...!", errorMessage, "Voltar", backToMenu);
    }

    public void OpenRankings(HighScores upgrades)
    {
        rankingsTab.SetActive(true);
        loadingPanel.StartLoading("Carregando...");
        StartCoroutine(NetworkManager.GetRequest<HighScores>("/highScores", LoadRankings, LoadError));
    }

    public void LoadRankings(HighScores rankings)
    {

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

        loadingPanel.StartLoading("Carregando...");
        StartCoroutine(NetworkManager.PostRequest<PlayerStats>("/player/upgrade", form, FinishUpgrade, LoadError));
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
            if(loadingPanel.gameObject.activeSelf)
                loadingPanel.gameObject.SetActive(false);

            Upgrade selectedStat = upgradeableStats[(int)selectedStatType];
            int level = playerStats[selectedStatType];

            uiReferences.currentValue.text = selectedStat.value[level].ToString();
            uiReferences.coins.text = playerStats.money.ToString();
            uiReferences.highscore.text = playerStats.highScore.ToString();

            if(level+1 < selectedStat.value.Length && level < selectedStat.cost.Length)
            {
                uiReferences.cost.text = selectedStat.cost[level].ToString();
                uiReferences.nextValue.text = selectedStat.value[level+1].ToString();
                if(playerStats.money < selectedStat.cost[level])
                    uiReferences.upgradeButton.enabled = false;
                else
                    uiReferences.upgradeButton.enabled = true;
            }
            else
            {
                uiReferences.nextValue.text = "MAX";
                uiReferences.upgradeButton.enabled = false;
                uiReferences.cost.text = "MAX";
            }

        }
    }
}
