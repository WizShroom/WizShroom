using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarHolder : MonoBehaviour
{
    public List<ActionBarSlot> actionBarSlots;
    public PlayerMovement playerMovement;
    public PlayerController playerController;

    bool paused;

    private void Awake()
    {
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
    }

    private void OnDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    private void Update()
    {
        if (paused)
        {
            return;
        }

        for (int i = 0; i < actionBarSlots.Count; i++)
        {
            if (!Input.GetKeyDown(actionBarSlots[i].key) || !actionBarSlots[i].equippedSpell)
            {
                continue;
            }

            if (playerMovement.engagedEnemy)
            {
                actionBarSlots[i].CastSpell(playerController, playerMovement.engagedEnemy);
            }
            else
            {
                Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;
                Vector3 castDirection = (groundPosition - playerController.transform.position).normalized;
                actionBarSlots[i].CastSpell(playerController, castDirection);
            }
        }
    }


    public void OnEventReceived(GameObject source, EVENT receivedEvent)
    {
        if (receivedEvent == EVENT.PAUSED)
        {
            OnPaused();
        }
        if (receivedEvent == EVENT.RESUMED)
        {
            OnResumed();
        }
    }

    public void OnPaused()
    {
        paused = true;
    }

    public void OnResumed()
    {
        paused = false;
    }
}
