using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarHolder : MonoBehaviour
{
    public List<ActionBarSlot> actionBarSlots;
    public PlayerMovement playerMovement;
    public PlayerController playerController;

    private void Update()
    {
        for (int i = 0; i < actionBarSlots.Count; i++)
        {
            if (Input.GetKeyDown(actionBarSlots[i].key))
            {
                if (actionBarSlots[i].equippedSpell)
                {
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
        }
    }
}
