using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsResetButton : MonoBehaviour
{
    public List<Quest> questsToReset;

    public void ResetQuests()
    {
        foreach (Quest quest in questsToReset)
        {
            quest.completedQuest = false;
            quest.failedQuest = false;
            for (int i = 0; i < quest.questSegments.Count; i++)
            {
                QuestSegment questSegment = quest.questSegments[i];
                questSegment.completedSegment = false;
                quest.questSegments[i] = questSegment;
                for (int j = 0; j < quest.questSegments[i].requiredItems.Count; j++)
                {
                    QuestItem questItem = questSegment.requiredItems[j];
                    questItem.itemDone = false;
                    questSegment.requiredItems[j] = questItem;
                }
                for (int j = 0; j < quest.questSegments[i].requiredKills.Count; j++)
                {
                    QuestKill questKill = questSegment.requiredKills[j];
                    questKill.currentKills = 0;
                    questKill.killDone = false;
                    questSegment.requiredKills[j] = questKill;
                }
            }
        }
    }
}