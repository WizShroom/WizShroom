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

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Dialogue nextDialogue;
    [SerializeField] Image mushSprite;
    [SerializeField] Image talkerSprite;

    bool talking = false;
    bool canSkipDialogue = false;

    private void Start()
    {
        hintImage.SetActive(false);
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

            if (dialogue.sendSignalAfterPart)
            {
                GameSignalHandler.Instance.SendSignal(gameObject, dialogue.signalToSend);
            }
            yield return null;
        }
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.RESUMED);
        UIHandler.Instance.DisableUIByType(UIType.Dialogue);
        UIHandler.Instance.EnableUIByType(UIType.InGame);
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