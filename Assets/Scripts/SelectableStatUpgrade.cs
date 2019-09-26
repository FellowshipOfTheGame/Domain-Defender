using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectableStatUpgrade : MonoBehaviour, ISelectHandler
{
    public StatType statType;
    [SerializeField] string measurementUnit;
    [SerializeField] TextMeshProUGUI measurementUnitText;

    public void OnSelect(BaseEventData data)
    {
        if(StatUpgradeMenu.instance != null)
        {
            StatUpgradeMenu.instance.Select(statType, this.gameObject);
            measurementUnitText.text = measurementUnit;
        }
        else
            Debug.LogWarning("There is no instance of StatUpgradeMenu. Have you forgotten to add it to the scene?");
    }
}
