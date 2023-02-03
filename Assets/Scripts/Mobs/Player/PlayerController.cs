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

    public override void MobKilled(MobController killedMob)
    {
        base.MobKilled(killedMob);
        CheckQuestsMob(killedMob);
    }

    public void CheckQuestsMob(MobController killedMob)
    {
        foreach (Quest questToCheck in currentQuests)
        {
            CheckQuestMobRequirements(questToCheck, killedMob);
        }
    }

    public void CheckQuestsItem(Item givenItem, int itemTotal)
    {
        foreach (Quest questToCheck in currentQuests)
        {
            CheckQuestItemRequirements(questToCheck, givenItem, itemTotal);
        }
    }

    public void CheckQuestItemRequirements(Quest questToCheck, Item aquiredItem, int itemTotal)
    {
        for (int i = 0; i < questToCheck.questSegments.Count; i++)
        {
            QuestSegment questSegment = questToCheck.questSegments[i];
            for (int j = 0; j < questSegment.requiredItems.Count; j++)
            {
                QuestItem questItem = questSegment.requiredItems[j];
                if (aquiredItem != questItem.itemToTake)
                {
                    continue;
                }
                if (itemTotal >= questItem.itemAmount)
                {
                    questItem.itemDone = true;
                }
            }
            CheckSegmentRequirement(questSegment);
        }
        CheckQuestDone(questToCheck);
    }

    public void CheckQuestMobRequirements(Quest questToCheck, MobController killedMob)
    {
        for (int i = 0; i < questToCheck.questSegments.Count; i++)
        {
            QuestSegment questSegment = questToCheck.questSegments[i];
            for (int j = 0; j < questSegment.requiredKills.Count; j++)
            {
                QuestKill questKill = questSegment.requiredKills[j];
                if (killedMob.mobID == questKill.monsterID)
                {
                    questKill.currentKills++;
                }

                if (questKill.currentKills >= questKill.killAmount)
                {
                    questKill.killDone = true;
                }
            }
            CheckSegmentRequirement(questSegment);
        }
        CheckQuestDone(questToCheck);
    }

    public void CheckSegmentRequirement(QuestSegment segment)
    {
        bool itemDone = true;
        bool killDone = true;
        foreach (QuestItem questItem in segment.requiredItems)
        {
            if (!questItem.itemDone)
            {
                itemDone = false;
            }
        }
        foreach (QuestKill questKill in segment.requiredKills)
        {
            if (!questKill.killDone)
            {
                killDone = false;
            }
        }
        if (itemDone && killDone)
        {
            segment.completedSegment = true;
        }
    }

    public void CheckQuestDone(Quest questToCheck)
    {
        bool questDone = true;
        foreach (QuestSegment questSegment in questToCheck.questSegments)
        {
            if (!questSegment.completedSegment)
            {
                questDone = false;
            }
        }
        if (questDone)
        {
            questToCheck.completedQuest = true;
        }
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
