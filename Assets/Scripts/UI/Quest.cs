using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject
{
    [TextArea(5, 5)]
    public string questDescription;
    public bool completedQuest;
    public List<QuestSegment> questSegments = new List<QuestSegment>();
}
[System.Serializable]
public struct QuestSegment
{
    public bool completedSegment;
    [TextArea(5, 5)]
    public string segmentDescription;
    public List<QuestItem> requiredItems;
}

[System.Serializable]
public struct QuestItem
{
    public string itemID;
    public int itemAmount;
}