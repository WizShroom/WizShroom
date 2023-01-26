using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterScreenType : UITypeController
{

    public TextMeshProUGUI characterLevel;
    public TextMeshProUGUI characterTitle;
    public TitleHolder currentTitle;
    public Image expBar;

    public void UpdateTitle(MobController targetMob, TitleHolder title)
    {
        if (currentTitle)
        {
            currentTitle.RemoveStatBoost(targetMob);
        }
        currentTitle = title;
        currentTitle.ApplyStatBoost(targetMob);
        characterTitle.text = currentTitle.titleName;
    }

    public void UpdateExpBar(float currentExp, int maxExp, int currentLevel)
    {
        expBar.fillAmount = currentExp / maxExp;
        characterLevel.text = "Level: " + currentLevel.ToString();
    }

}