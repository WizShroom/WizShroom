using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonMono<Inventory>
{

    public GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots;

    public int slotAmount = 40;

    private void Start()
    {
        InventoryScreenType inventoryScreen = (InventoryScreenType)UIHandler.Instance.GetUITypeControllerByType(UIType.Inventory);
        Transform inventorySlotContainer = inventoryScreen.inventorySlotContainer;
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject inventorySlotEntity = Instantiate(inventorySlotPrefab, inventorySlotContainer);
            InventorySlot inventorySlot = inventorySlotEntity.GetComponent<InventorySlot>();
            inventorySlot.slotID = i;
            inventorySlots.Add(inventorySlot);
        }
    }

    public void AddItemToFreeSlot(Item itemToAdd, int addAmount = 1)
    {
        int nextSlotAddAmount = addAmount;
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            nextSlotAddAmount = inventorySlot.AddItem(itemToAdd, nextSlotAddAmount);
            if (nextSlotAddAmount <= 0)
            {
                break;
            }
        }
    }

    public void MergeSlots(int giverSlotID, int takerSlotID)
    {
        InventorySlot giverSlot = GetInventorySlotByID(giverSlotID);
        InventorySlot takerSlot = GetInventorySlotByID(takerSlotID);

        int remainingAmount = takerSlot.AddItem(giverSlot.containedItem.item, giverSlot.containedItem.itemAmount);
        if (remainingAmount > 0)
        {
            giverSlot.SetItem(giverSlot.containedItem.item, remainingAmount);
        }
        else
        {
            giverSlot.RemoveItem(removeCompletely: true);
        }
    }

    public InventorySlot GetInventorySlotByID(int slotID)
    {
        InventorySlot returnSlot = null;
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.slotID == slotID)
            {
                returnSlot = inventorySlot;
                break;
            }
        }
        return returnSlot;
    }

}
