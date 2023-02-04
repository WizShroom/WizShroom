using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [Header("Dialogue")]
    public bool checkQuestRequirement;
    public Quest questToCheck;
    public List<DialogueSegment> dialogueSegments = new List<DialogueSegment>();
}

[System.Serializable]
public struct DialogueSegment
{
    [TextArea(5, 5)]
    public string dialogueText;
    public float dialogueDisplayTime;
    public bool isMushTalking;
    public Sprite talkerSprite;
    public Sprite mushSprite;
    public ChoiceAndQuest choiceAndQuest;

    public bool startNewDirectly;
    public Dialogue newDialogue;

    public DialogueAnimationControls dialogueAnimationControls;

    public bool sendSignalAfterPart;
    public string signalToSend;

    public bool clickable;
    public bool disableControllerAfter;
}

[System.Serializable]
public struct ChoiceAndQuest
{
    public bool hasChoiceToMake;
    public DialogueChoice dialogueChoice;
    public bool canChooseQuest;
    public bool questRequirePositiveOutcome;
    public Quest questToGive;
}

[System.Serializable]
public struct DialogueAnimationControls
{
    public bool waitForAnimation;
    public bool animationBeforeDialogue;
    public ScriptedAnimation animationToPlay;
}

[System.Serializable]
public struct DialogueChoice
{
    [TextArea(5, 5)]
    public string dialogueChoiceText;
    public Dialogue positiveDialogue;
    public bool positiveDisableController;
    public bool positiveEnableController;
    public Dialogue negativeDialogue;
    public bool negativeDisableController;
    public bool negativeEnableController;
}