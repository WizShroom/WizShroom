using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellSlot : EventTrigger
{
    public int slotID;
    public Image slotImage;
    public SpellSlotItem containedSpell;

    public bool ContainSpell()
    {
        if (containedSpell.spell == null)
        {
            return false;
        }
        return true;
    }

    public bool AddSpell(Spell spellToAdd)
    {
        if (ContainSpell())
        {
            return false;
        }
        containedSpell.spell = spellToAdd;
        containedSpell.spellName = spellToAdd.spellName;
        containedSpell.spellSprite = spellToAdd.spellSprite;
        slotImage.sprite = containedSpell.spellSprite;
        return true;
    }

    public void RemoveSpell()
    {
        containedSpell.spell = null;
        containedSpell.spellName = "";
        containedSpell.spellSprite = null;
        slotImage.sprite = containedSpell.spellSprite;
    }

    public void SetSpell(Spell spellToSet)
    {
        containedSpell.spell = spellToSet;
        containedSpell.spellName = spellToSet.spellName;
        containedSpell.spellSprite = spellToSet.spellSprite;
        slotImage.sprite = containedSpell.spellSprite;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Use the event data raycast results to see if we hit another slot
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject otherObj = eventData.pointerCurrentRaycast.gameObject;
            if (otherObj.CompareTag("SpellSlot"))
            {
                SpellSlot otherSlot = otherObj.GetComponent<SpellSlot>();
                if (otherSlot != null && otherSlot != this)
                {
                    SpellsInventory.Instance.CheckMergeability(slotID, otherSlot.slotID);
                }
            }
            else if (otherObj.CompareTag("ActionBarSlot"))
            {
                ActionBarSlot otherSlot = otherObj.GetComponent<ActionBarSlot>();
                if (otherSlot != null)
                {
                    otherSlot.AddSpell(containedSpell.spell);
                }
            }

        }
    }
}

[System.Serializable]
public struct SpellSlotItem
{
    public string spellName;
    public Sprite spellSprite;
    public Spell spell;
}
