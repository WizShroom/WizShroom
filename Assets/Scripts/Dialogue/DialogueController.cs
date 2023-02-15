using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public bool disabled = false;
    public GameObject hintImage;
    bool canActivate = false;
    TextMeshProUGUI dialogueText;
    Image mushSprite;
    Image talkerSprite;
    Button positiveResponseButton;
    Button negativeResponseButton;
    bool choosing = false;

    public Dialogue currentDialogue;
    public Dialogue nextDialogue;

    public Dialogue remainOnCurrentDialogue;

    bool talking = false;
    bool canSkipDialogue = false;
    bool isPositiveChoice = false;

    private void Start()
    {
        DialogueScreenType dialogueScreenType = (DialogueScreenType)UIHandler.Instance.GetUITypeControllerByType(UIType.Dialogue);
        dialogueText = dialogueScreenType.dialogueText;
        mushSprite = dialogueScreenType.mushTalking;
        talkerSprite = dialogueScreenType.otherTalking;
        positiveResponseButton = dialogueScreenType.positiveResponse;
        negativeResponseButton = dialogueScreenType.negativeResponse;
        positiveResponseButton.gameObject.SetActive(false);
        negativeResponseButton.gameObject.SetActive(false);
        if (hintImage)
        {
            hintImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (talking && !canSkipDialogue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            canSkipDialogue = true;
            return;
        }

        if (disabled || !canActivate)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDialogue();
            disabled = true;
            hintImage.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        if (nextDialogue != remainOnCurrentDialogue && nextDialogue != null)
        {
            currentDialogue = nextDialogue;
            nextDialogue = remainOnCurrentDialogue;
        }

        StartCoroutine(Talk(currentDialogue));
    }

    IEnumerator Talk(Dialogue dialogueToDisplay)
    {
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.PAUSED);

        talking = true;

        bool initiateNextDialogue = false;
        bool hasChoice = false;
        bool toDisable = false;
        Dialogue dialogueContinuation = null;

        yield return null;
        UIHandler.Instance.EnableUIByType(UIType.Dialogue);
        UIHandler.Instance.DisableUIByType(UIType.InGame);

        if (dialogueToDisplay.checkQuestRequirement && dialogueToDisplay.questToCheck)
        {
            if (dialogueToDisplay.questToCheck.completedQuest)
            {
                dialogueToDisplay = dialogueToDisplay.questToCheck.questCompleted;
            }
            else if (dialogueToDisplay.questToCheck.completedQuest)
            {
                dialogueToDisplay = dialogueToDisplay.questToCheck.questOnGoing;
            }
            else if (dialogueToDisplay.questToCheck.failedQuest)
            {
                dialogueToDisplay = dialogueToDisplay.questToCheck.questFailed;
            }
        }

        foreach (DialogueSegment dialogueSegment in dialogueToDisplay.dialogueSegments)
        {
            if (dialogueSegment.disableControllerAfter)
            {
                toDisable = true;
            }
            dialogueContinuation = dialogueSegment.newDialogue;
            string dialogueTextToDisplay = dialogueSegment.dialogueText;
            dialogueText.text = "";
            if (dialogueSegment.isMushTalking)
            {
                mushSprite.enabled = true;
                mushSprite.sprite = dialogueSegment.mushSprite;
                talkerSprite.enabled = false;
            }
            else
            {
                mushSprite.enabled = false;
                talkerSprite.enabled = true;
                talkerSprite.sprite = dialogueSegment.talkerSprite;
            }

            choosing = false;
            hasChoice = dialogueSegment.choiceAndQuest.hasChoiceToMake;
            if (hasChoice)
            {
                dialogueTextToDisplay = dialogueSegment.choiceAndQuest.dialogueChoice.dialogueChoiceText;
                choosing = true;
            }

            for (int i = 0; i < dialogueTextToDisplay.Length; i++)
            {
                if (dialogueSegment.clickable)
                {
                    if (canSkipDialogue)
                    {
                        canSkipDialogue = false;
                        dialogueText.text = dialogueTextToDisplay;
                        break;
                    }
                }
                canSkipDialogue = false;
                dialogueText.text += dialogueTextToDisplay[i];
                yield return new WaitForSeconds(0.1f);
            }
            float startTime = Time.time;
            while (Time.time - startTime < dialogueSegment.dialogueDisplayTime)
            {
                if (dialogueSegment.clickable)
                {
                    if (canSkipDialogue)
                    {
                        canSkipDialogue = false;
                        break;
                    }
                }
                canSkipDialogue = false;
                yield return null;
            }

            yield return null;

            if (choosing)
            {
                positiveResponseButton.gameObject.SetActive(true);
                negativeResponseButton.gameObject.SetActive(true);
                positiveResponseButton.onClick.RemoveAllListeners();
                negativeResponseButton.onClick.RemoveAllListeners();
                positiveResponseButton.onClick.AddListener(() => OnChoiceMade(true));
                negativeResponseButton.onClick.AddListener(() => OnChoiceMade(false));
            }

            while (choosing)
            {
                yield return null;
            }

            if (dialogueSegment.dialogueAnimationControls.animationToPlay != null && !dialogueSegment.dialogueAnimationControls.animationBeforeDialogue)
            {
                if (dialogueSegment.dialogueAnimationControls.waitForAnimation)
                {
                    UIHandler.Instance.DisableUIByType(UIType.Dialogue);
                    yield return StartCoroutine(dialogueSegment.dialogueAnimationControls.animationToPlay.AnimateAll());
                    UIHandler.Instance.EnableUIByType(UIType.Dialogue);
                }
                else
                {
                    StartCoroutine(dialogueSegment.dialogueAnimationControls.animationToPlay.AnimateAll());
                }
            }

            if (dialogueSegment.sendSignalAfterPart)
            {
                GameSignalHandler.Instance.SendSignal(gameObject, dialogueSegment.signalToSend);
            }

            if (hasChoice)
            {
                if (isPositiveChoice)
                {
                    dialogueContinuation = dialogueSegment.choiceAndQuest.dialogueChoice.positiveDialogue;
                    if (dialogueSegment.choiceAndQuest.canChooseQuest && dialogueSegment.choiceAndQuest.questRequirePositiveOutcome)
                    {
                        PlayerController playerController = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<PlayerController>();
                        playerController.currentQuests.Add(dialogueSegment.choiceAndQuest.questToGive);
                    }
                    if (dialogueSegment.choiceAndQuest.dialogueChoice.positiveEnableController)
                    {
                        disabled = false;
                    }
                    if (dialogueSegment.choiceAndQuest.dialogueChoice.positiveDisableController)
                    {
                        disabled = true;
                    }
                }
                else
                {
                    dialogueContinuation = dialogueSegment.choiceAndQuest.dialogueChoice.negativeDialogue;
                    if (dialogueSegment.choiceAndQuest.canChooseQuest && !dialogueSegment.choiceAndQuest.questRequirePositiveOutcome)
                    {
                        PlayerController playerController = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<PlayerController>();
                        playerController.currentQuests.Add(dialogueSegment.choiceAndQuest.questToGive);
                    }
                    if (dialogueSegment.choiceAndQuest.dialogueChoice.negativeEnableController)
                    {
                        disabled = false;
                    }
                    if (dialogueSegment.choiceAndQuest.dialogueChoice.negativeDisableController)
                    {
                        disabled = true;
                    }
                }

                if (!dialogueSegment.choiceAndQuest.canChooseQuest)
                {
                    PlayerController playerController = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<PlayerController>();
                    playerController.currentQuests.Add(dialogueSegment.choiceAndQuest.questToGive);
                }
            }

            TransitionToDialogue(dialogueContinuation);
            if (dialogueSegment.startNewDirectly)
            {
                initiateNextDialogue = true;
            }

        }

        if (initiateNextDialogue)
        {
            StartDialogue();
        }
        else
        {
            GameEventHandler.Instance.SendEvent(gameObject, EVENT.RESUMED);
            UIHandler.Instance.DisableUIByType(UIType.Dialogue);
            UIHandler.Instance.EnableUIByType(UIType.InGame);
            disabled = toDisable;
        }

        talking = false;

        yield return null;
    }

    public void TransitionToDialogue(Dialogue transitionDialogue)
    {
        if (transitionDialogue == null || transitionDialogue == remainOnCurrentDialogue)
        {
            return;
        }

        nextDialogue = transitionDialogue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disabled)
        {
            if (hintImage)
            {
                hintImage.SetActive(false);
            }
            return;
        }
        if (other.CompareTag("Player"))
        {
            canActivate = true;
            if (hintImage)
            {
                hintImage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (disabled)
        {
            if (hintImage)
            {
                hintImage.SetActive(false);
            }
            return;
        }

        if (other.CompareTag("Player"))
        {
            canActivate = false;
            if (hintImage)
            {
                hintImage.SetActive(false);
            }
        }
    }

    public void OnChoiceMade(bool isPositiveResponse)
    {
        isPositiveChoice = isPositiveResponse;
        positiveResponseButton.gameObject.SetActive(false);
        negativeResponseButton.gameObject.SetActive(false);
        choosing = false;
    }

}