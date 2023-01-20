using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [Header("Dialogue")]
    public List<DialogueSegment> dialogueSegments = new List<DialogueSegment>();

}

[System.Serializable]
public struct DialogueSegment
{
    public string dialogueText;
    public float dialogueDisplayTime;
    public bool isMushTalking;
    public Sprite talkerSprite;
    public Sprite mushSprite;
    public List<DialogueChoice> dialogueChoices;

    public bool sendSignalAfterPart;
    public string signalToSend;

    public bool clickable;
}

[System.Serializable]
public struct DialogueChoice
{
    public string dialogueChoice;
    public Dialogue followOnDialogue;
}