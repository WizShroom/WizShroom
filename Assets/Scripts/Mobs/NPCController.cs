using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MobController
{
    public bool enableOnTrigger = false;
    public bool enableDialogueOnTrigger = true;
    public string signalToTrigger;
    public SpriteRenderer ourRenderer;
    DialogueController dialogueController;

    public Dialogue startingDialogue;

    public override void Initialize()
    {
        dialogueController = GetComponent<DialogueController>();
        base.Initialize();
        if (enableOnTrigger)
        {
            ourRenderer.enabled = false;
        }
    }

    public override void OnAwake()
    {
        base.OnAwake();
        GameSignalHandler.Instance.OnSignalReceived += OnSignalReceived;
    }

    public override void OnObjDestroy()
    {
        GameSignalHandler.Instance.OnSignalReceived -= OnSignalReceived;
        base.OnObjDestroy();
    }

    public void OnSignalReceived(GameObject source, string signal)
    {
        if (signal == signalToTrigger)
        {
            ourRenderer.enabled = true;
            dialogueController.StartDialogue(startingDialogue);
        }
    }

}