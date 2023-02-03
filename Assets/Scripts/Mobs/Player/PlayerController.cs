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
        for (int i = 0; i < currentQuests.Count; i++)
        {
            Quest questToCheck = currentQuests[i];
            CheckQuestMobRequirements(ref questToCheck, killedMob);
        }
    }

    public void CheckQuestsItem(Item givenItem, int itemTotal)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            Quest questToCheck = currentQuests[i];
            CheckQuestItemRequirements(ref questToCheck, givenItem, itemTotal);
        }
    }

    public void CheckQuestItemRequirements(ref Quest questToCheck, Item aquiredItem, int itemTotal)
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
                    questSegment = new QuestSegment
                    {
                        completedSegment = questSegment.completedSegment,
                        segmentDescription = questSegment.segmentDescription,
                        isOptional = questSegment.isOptional,
                        requiredItems = new List<QuestItem>(questSegment.requiredItems),
                        requiredKills = new List<QuestKill>(questSegment.requiredKills)
                    };
                    questSegment.requiredItems[j] = new QuestItem
                    {
                        itemDone = true,
                        itemToTake = questItem.itemToTake,
                        itemAmount = questItem.itemAmount
                    };
                    questToCheck.questSegments[i] = questSegment;
                }
            }
            CheckSegmentCompletion(ref questSegment);
            questToCheck.questSegments[i] = questSegment;
        }
        CheckQuestDone(ref questToCheck);
    }

    public void CheckQuestMobRequirements(ref Quest questToCheck, MobController killedMob)
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
                questSegment.requiredKills[j] = questKill;
            }
            CheckSegmentCompletion(ref questSegment);
            questToCheck.questSegments[i] = questSegment;
        }
        CheckQuestDone(ref questToCheck);
    }

    private void CheckSegmentCompletion(ref QuestSegment questSegment)
    {
        bool itemDone = true;
        bool killDone = true;
        foreach (QuestItem questItem in questSegment.requiredItems)
        {
            if (!questItem.itemDone)
            {
                itemDone = false;
            }
        }
        foreach (QuestKill questKill in questSegment.requiredKills)
        {
            if (!questKill.killDone)
            {
                killDone = false;
            }
        }
        if (itemDone && killDone)
        {
            questSegment.completedSegment = true;
        }
    }

    public void CheckQuestDone(ref Quest questToCheck)
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
