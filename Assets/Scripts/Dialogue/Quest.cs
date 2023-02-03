using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject
{
    [TextArea(5, 5)]
    public string questDescription;
    public bool completedQuest;
    public bool failedQuest;
    public List<QuestSegment> questSegments = new List<QuestSegment>();
    public Dialogue questOnGoing;
    public Dialogue questCompleted;
    public Dialogue questFailed;
}
[System.Serializable]
public struct QuestSegment
{
    public bool completedSegment;
    [TextArea(5, 5)]
    public string segmentDescription;
    public bool isOptional;
    public List<QuestItem> requiredItems;
    public List<QuestKill> requiredKills;
}

[System.Serializable]
public struct QuestItem
{
    [SerializeField] public bool itemDone;
    [SerializeField] public Item itemToTake;
    [SerializeField] public int itemAmount;
}

[System.Serializable]
public struct QuestKill
{
    public bool killDone;
    public string monsterID;
    public int currentKills;
    public int killAmount;
}