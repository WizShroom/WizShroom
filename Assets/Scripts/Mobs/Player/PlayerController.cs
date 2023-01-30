using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MobController
{
    public string playerName;

    public int availableStatPoints;

    public int statPointPerLevel = 5;

    public StatUIHolder statUIHolder;
    public ActionBarHolder actionBarHolder;

    public TitleHolder initialTitle;
    CharacterScreenType characterScreenType;
    InGameScreenType inGameScreenType;

    public List<Quest> currentQuests;

    public override void Initialize()
    {
        base.Initialize();
        statUIHolder.ConnectStats(this);
        actionBarHolder.playerMovement = GetComponent<PlayerMovement>();
        actionBarHolder.playerController = this;
        inGameScreenType = (InGameScreenType)UIHandler.Instance.GetUITypeControllerByType(UIType.InGame);
        UpdateIndicators();
        characterScreenType = (CharacterScreenType)UIHandler.Instance.GetUITypeControllerByType(UIType.CharacterInfo);
        characterScreenType.UpdateExpBar(experience, experienceForLevelUp, currentLevel);
        characterScreenType.UpdateTitle(this, initialTitle);
    }

    public override void UpdateIndicators()
    {
        if (!inGameScreenType)
        {
            return;
        }
        inGameScreenType.UpdateHealth(health, maxHealth);
        inGameScreenType.UpdateMana(mana, maxMana);
        inGameScreenType.UpdateExpBar(experience, experienceForLevelUp, currentLevel);
    }

    public override void IncreaseExperience(float increaseAmount)
    {
        base.IncreaseExperience(increaseAmount);
        characterScreenType.UpdateExpBar(experience, experienceForLevelUp, currentLevel);
        inGameScreenType.UpdateExpBar(experience, experienceForLevelUp, currentLevel);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        availableStatPoints += statPointPerLevel;
        characterScreenType.UpdateExpBar(experience, experienceForLevelUp, currentLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (!interactable.clickable && !interactable.disabled)
            {
                interactable.Interact(this);
            }
        }
    }
}
