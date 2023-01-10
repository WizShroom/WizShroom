using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUIHolder : MonoBehaviour
{
    public List<StatUI> stats = new List<StatUI>();

    public PlayerController playerController;

    public void ConnectStats(PlayerController playerController)
    {
        if (!this.playerController)
        {
            this.playerController = playerController;
        }
        UpdateStatUIs();
    }

    public void UpdateStatUI(StatUI statUI, Stat stat)
    {
        statUI.holder = this;

        if (playerController.availableStatPoints <= 0)
        {
            statUI.increaseValueButton.gameObject.SetActive(false);
        }
        else
        {
            statUI.increaseValueButton.gameObject.SetActive(true);
        }

        statUI.statAmount = stat.GetValue();
        statUI.statNameText.text = statUI.statType.ToString() + ":";
        statUI.statAmountText.text = stat.GetValue().ToString();
    }

    public void UpdateStatUIs()
    {
        foreach (Stat stat in playerController.stats)
        {
            foreach (StatUI statUI in stats)
            {
                if (statUI.statType != stat.statType)
                {
                    continue;
                }
                UpdateStatUI(statUI, stat);
                break;
            }
        }
    }
}
