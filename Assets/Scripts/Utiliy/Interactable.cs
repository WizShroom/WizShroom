using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool clickable = false;
    public int distanceForInteraction = 1;

    public bool triggered = false;
    public bool reversable = false;

    public bool disabled = false;
    public bool disableOnTrigger = false;

    public bool canSendSignal = true;
    public bool alwaysSendSignals = true;
    public bool onlyRegisteredInteractionSignal;

    public List<string> triggerOnSignals;
    public List<string> disableOnSignals;
    public List<string> enableOnSignals;

    public List<string> signalsToSend;

    public delegate void InteractSignal();
    public event InteractSignal OnInteracted;

    private void Awake()
    {
        GameSignalHandler.Instance.OnSignalReceived += OnSignalReceived;
    }

    private void OnDestroy()
    {
        GameSignalHandler.Instance.OnSignalReceived -= OnSignalReceived;
    }

    public void Interact(MobController interactingMob = null, bool triggeredBySignal = false)
    {
        if (!triggeredBySignal && (disabled || (!reversable && triggered)))
        {
            return;
        }

        if (canSendSignal && (!triggeredBySignal || alwaysSendSignals))
        {
            if (onlyRegisteredInteractionSignal)
            {
                OnInteracted?.Invoke();
                triggered = !triggered;
                if (disableOnTrigger)
                {
                    disabled = true;
                }
                return;
            }
            foreach (string SignalToSend in signalsToSend)
            {
                GameSignalHandler.Instance.SendSignal(gameObject, SignalToSend);
            }
        }

        triggered = !triggered;
        if (disableOnTrigger)
        {
            disabled = true;
        }
    }

    public void OnSignalReceived(GameObject source, string signal)
    {
        foreach (string triggerOnSignal in triggerOnSignals)
        {
            if (signal == triggerOnSignal)
            {
                Interact(triggeredBySignal: true);
            }
        }
        foreach (string disableOnSignal in disableOnSignals)
        {
            if (signal == disableOnSignal)
            {
                disabled = true;
            }
        }
        foreach (string enableOnSIgnal in enableOnSignals)
        {
            if (signal == enableOnSIgnal)
            {
                disabled = false;
            }
        }
    }

}
