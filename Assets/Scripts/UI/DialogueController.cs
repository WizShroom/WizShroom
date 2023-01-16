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
    [SerializeField] Dialogue startDialogue;
    [SerializeField] Image mushSprite;
    [SerializeField] Image talkerSprite;

    private void Start()
    {
        hintImage.SetActive(false);
    }

    private void Update()
    {
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
        StartCoroutine(DisplayDialogue(startDialogue));
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        StartCoroutine(DisplayDialogue(_dialogue));
    }

    IEnumerator DisplayDialogue(Dialogue _dialogue)
    {
        yield return null;
        Debug.Log("Starting Dialogue Chain");

        UIHandler.Instance.EnableUIByType(UIType.Dialogue);
        UIHandler.Instance.DisableUIByType(UIType.InGame);
        foreach (DialogueSegment dialogue in _dialogue.dialogueSegments)
        {
            dialogueText.text = dialogue.dialogueText;
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
            yield return new WaitForSeconds(dialogue.dialogueDisplayTime);

            if (dialogue.sendSignalAfterPart)
            {
                GameSignalHandler.Instance.SendSignal(gameObject, dialogue.signalToSend);
            }
        }
        UIHandler.Instance.DisableUIByType(UIType.Dialogue);
        UIHandler.Instance.EnableUIByType(UIType.InGame);
        Debug.Log("Ending Dialogue Chain");
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