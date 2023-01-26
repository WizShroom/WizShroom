using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameScreenType : UITypeController
{

    public Image playerHealth;
    public Image playerMana;
    public Image playerExp;
    public TextMeshProUGUI playerLevel;

    public void UpdateHealth(float healthAmount, int maxHealth)
    {
        playerHealth.fillAmount = healthAmount / maxHealth;
    }

    public void UpdateMana(float manaAmount, int maxMana)
    {
        playerMana.fillAmount = manaAmount / maxMana;
    }

    public void UpdateExpBar(float experience, int maxExperience, int currentLevel)
    {
        playerLevel.text = "Level: " + currentLevel;
        playerExp.fillAmount = experience / maxExperience;
    }

}