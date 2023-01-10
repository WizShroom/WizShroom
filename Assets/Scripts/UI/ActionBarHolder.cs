using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarHolder : MonoBehaviour
{
    public List<ActionBarSlot> actionBarSlots;
    public PlayerMovement playerMovement;
    public PlayerController playerController;
    public LayerMask groundLayer;

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
                        Vector3 mousePos = Input.mousePosition;
                        Ray ray = Camera.main.ScreenPointToRay(mousePos);
                        RaycastHit hit;
                        Vector3 hitPoint = default(Vector3);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                        {
                            hitPoint = hit.point;
                        }
                        Vector3 castDirection = (hitPoint - playerController.transform.position).normalized;
                        actionBarSlots[i].CastSpell(playerController, castDirection);
                    }
                }
            }
        }
    }
}
