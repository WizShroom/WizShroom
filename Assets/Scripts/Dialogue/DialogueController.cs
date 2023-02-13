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

    public Dialogue currentDialogue;
    public Dialogue nextDialogue;

    public Dialogue remainOnCurrentDialogue;

    bool talking = false;
    bool canSkipDialogue = false;

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
        if (talking && !canSkipDialogue && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)))
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
        if (nextDialogue != remainOnCurrentDialogue)
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

        yield return null;
        UIHandler.Instance.EnableUIByType(UIType.Dialogue);
        UIHandler.Instance.DisableUIByType(UIType.InGame);

        foreach (DialogueSegment dialogueSegment in dialogueToDisplay.dialogueSegments)
        {
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
        }

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

}