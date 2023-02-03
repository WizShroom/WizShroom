using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsInventory : SingletonMono<SpellsInventory>
{
    public List<SpellSlot> spellSlots;

    private void Start()
    {
        for (int i = 0; i < spellSlots.Count; i++)
        {
            SpellSlot spellSlot = spellSlots[i];
            spellSlot.slotID = i;
        }
    }

    public void AddSpellToFreeSlot(Spell spellToAdd)
    {
        foreach (SpellSlot spellSlot in spellSlots)
        {
            if (spellSlot.AddSpell(spellToAdd))
            {
                break;
            }
        }
    }

    public void CheckMergeability(int giverSlotID, int takerSlotID)
    {
        SpellSlot giverSlot = GetSpellSlotByID(giverSlotID);
        SpellSlot takerSlot = GetSpellSlotByID(takerSlotID);

        if (takerSlot.containedSpell.spell == null)
        {
            MoveSpell(giverSlot, takerSlot);
        }
    }

    public void MoveSpell(SpellSlot giverSlot, SpellSlot takerSlot)
    {
        takerSlot.AddSpell(giverSlot.containedSpell.spell);
        giverSlot.RemoveSpell();
    }

    public SpellSlot GetSpellSlotByID(int slotID)
    {
        SpellSlot returnSlot = null;
        foreach (SpellSlot spellSlot in spellSlots)
        {
            if (spellSlot.slotID == slotID)
            {
                returnSlot = spellSlot;
                break;
            }
        }
        return returnSlot;
    }
}