using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour
{
    public string statName;
    public StatType statType;
    public int statAmount;

    public Button increaseValueButton;

    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statAmountText;

    [HideInInspector] public StatUIHolder holder;

    private void Awake()
    {
        statName = statType.ToString();

        increaseValueButton.onClick.AddListener(() => IncreaseStatType(statType));
    }

    public void IncreaseStatType(StatType statType)
    {
        holder.playerController.availableStatPoints -= 1;
        holder.playerController.IncreaseStatValue(statType);
        holder.UpdateStatUIs();
    }
}
