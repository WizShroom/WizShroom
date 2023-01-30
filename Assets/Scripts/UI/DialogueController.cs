using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public bool disabled;

    public GameObject hintImage;
    bool canActivate = false;

    TextMeshProUGUI dialogueText;
    Image mushSprite;
    Image talkerSprite;
    Button positiveResponseButton;
    Button negativeResponseButton;
    bool choosing;

    [SerializeField] Dialogue nextDialogue;

    bool talking = false;
    bool canSkipDialogue = false;
    bool isPositiveChoice = false;

    public bool triggeredBySignal = false;
    public string signalToTrigger;

    private void Awake()
    {
        GameSignalHandler.Instance.OnSignalReceived += OnSignalReceived;
    }

    private void OnDestroy()
    {
        GameSignalHandler.Instance.OnSignalReceived -= OnSignalReceived;
    }

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
        if (talking && !canSkipDialogue && Input.GetMouseButtonUp(0))
        {
            canSkipDialogue = true;
        }

        if (disabled || !canActivate)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateDialogue();
            disabled = true;
            hintImage.SetActive(false);
        }
    }

    public void ActivateDialogue()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        StartCoroutine(DisplayDialogue(nextDialogue));
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        StartCoroutine(DisplayDialogue(_dialogue));
    }

    IEnumerator DisplayDialogue(Dialogue _dialogue)
    {
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.PAUSED);
        yield return null;

        talking = true;
        bool hasChoice = false;
        Dialogue newDialogue = null;

        UIHandler.Instance.EnableUIByType(UIType.Dialogue);
        UIHandler.Instance.DisableUIByType(UIType.InGame);
        foreach (DialogueSegment dialogue in _dialogue.dialogueSegments)
        {
            string dialogueTextToDisplay = dialogue.dialogueText;
            dialogueText.text = "";
            if (dialogue.isMushTalking)
            {
                mushSprite.enabled = true;
                mushSprite.sprite = dialogue.mushSprite;
                talkerSprite.enabled = false;
            }
            else
            {
                mushSprite.enabled = false;
                talkerSprite.enabled = true;
                talkerSprite.sprite = dialogue.talkerSprite;
            }

            if (dialogue.dialogueAnimationControls.animationToPlay != null && dialogue.dialogueAnimationControls.animationBeforeDialogue)
            {
                if (dialogue.dialogueAnimationControls.waitForAnimation)
                {
                    UIHandler.Instance.DisableUIByType(UIType.Dialogue);
                    yield return GameController.Instance.StartCoroutine(dialogue.dialogueAnimationControls.animationToPlay.AnimateAll());
                    UIHandler.Instance.EnableUIByType(UIType.Dialogue);
                }
                else
                {
                    GameController.Instance.StartCoroutine(dialogue.dialogueAnimationControls.animationToPlay.AnimateAll());
                }
            }

            choosing = false;
            hasChoice = dialogue.choiceAndQuest.hasChoiceToMake;
            if (hasChoice)
            {
                dialogueTextToDisplay = dialogue.choiceAndQuest.dialogueChoice.dialogueChoiceText;
                choosing = true;
            }

            for (int i = 0; i < dialogueTextToDisplay.Length; i++)
            {
                if (dialogue.clickable)
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
            while (Time.time - startTime < dialogue.dialogueDisplayTime)
            {
                if (dialogue.clickable)
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
                positiveResponseButton.onClick.AddListener(() => OnChoiceMade(true));
                negativeResponseButton.onClick.AddListener(() => OnChoiceMade(false));
            }

            while (choosing)
            {
                yield return null;
            }

            if (dialogue.dialogueAnimationControls.animationToPlay != null && !dialogue.dialogueAnimationControls.animationBeforeDialogue)
            {
                if (dialogue.dialogueAnimationControls.waitForAnimation)
                {
                    UIHandler.Instance.DisableUIByType(UIType.Dialogue);
                    yield return StartCoroutine(dialogue.dialogueAnimationControls.animationToPlay.AnimateAll());
                    UIHandler.Instance.EnableUIByType(UIType.Dialogue);
                }
                else
                {
                    StartCoroutine(dialogue.dialogueAnimationControls.animationToPlay.AnimateAll());
                }
            }

            if (dialogue.sendSignalAfterPart)
            {
                GameSignalHandler.Instance.SendSignal(gameObject, dialogue.signalToSend);
            }
            yield return null;

            if (dialogue.newDialogue)
            {
                newDialogue = dialogue.newDialogue;
                break;
            }

            if (hasChoice)
            {
                if (positiveResponseButton)
                {
                    newDialogue = dialogue.choiceAndQuest.dialogueChoice.positiveDialogue;
                }
                else
                {
                    newDialogue = dialogue.choiceAndQuest.dialogueChoice.negativeDialogue;
                }
                break;
            }
        }

        if (!hasChoice || newDialogue == null)
        {
            GameEventHandler.Instance.SendEvent(gameObject, EVENT.RESUMED);
            UIHandler.Instance.DisableUIByType(UIType.Dialogue);
            UIHandler.Instance.EnableUIByType(UIType.InGame);
        }
        else
        {
            StartDialogue(newDialogue);
        }
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

    public void OnSignalReceived(GameObject source, string signalReceived)
    {
        if (signalReceived == signalToTrigger)
        {
            StartDialogue();
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