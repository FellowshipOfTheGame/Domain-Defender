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
        public Button upgradeButton;
    }

    [SerializeField] UIReferences uiReferences;

    [SerializeField] GameObject selectionIcon;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject defaultSelected;
    Upgrade selectedStat;

    private void Start()
    {
        Select(defaultSelected.GetComponent<SelectableStatUpgrade>().statType, defaultSelected);   
    }

    public void Select(PlayerStats.StatType selectedStatType, GameObject selectedObject)
    {
        selectionIcon.transform.position = selectedObject.transform.position;
        // selectedStat = playerStats.upgradeLevel[(int)selectedStatType];
        
        UpdateUI();
    }

    public void Upgrade()
    {
        // playerStats.Coins -= selectedStat.costs[selectedStat.level].subCosts[selectedStat.sublevel];
        // uiReferences.coins.text = playerStats.Coins.ToString();

        // UpdateUI();
    }

    private void UpdateUI()
    {
        // selectedStat.currentValue = selectedStat.values[selectedStat.level];
        // uiReferences.currentLevel.text = selectedStat.level.ToString();
        // uiReferences.nextLevel.text = (selectedStat.level + 1).ToString();
        // uiReferences.cost.text = selectedStat.costs[selectedStat.level].subCosts[selectedStat.sublevel].ToString();
        // uiReferences.currentValue.text = selectedStat.currentValue.ToString();
        // uiReferences.sublevelSlider.value = uiReferences.sublevelSlider.maxValue / selectedStat.numOfSubLevels * selectedStat.sublevel;

        // if(playerStats.Coins < selectedStat.costs[selectedStat.level].subCosts[selectedStat.sublevel])
        //     uiReferences.upgradeButton.enabled = false;
    }
}
