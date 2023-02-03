using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Collectable : MonoBehaviour
{
    public Item itemToCollect;
    public int containedItemAmount = 1;

    public Spell spellToCollect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemToCollect)
            {
                Inventory.Instance.AddItem(itemToCollect, containedItemAmount);
            }
            if (spellToCollect)
            {
                SpellsInventory.Instance.AddSpellToFreeSlot(spellToCollect);
            }
            Destroy(gameObject);
        }
    }
}