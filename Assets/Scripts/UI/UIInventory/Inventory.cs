using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonMono<Inventory>
{
    public List<InventorySlot> inventorySlots;

    public int miceliumAmount = 0;

    private void Start()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            inventorySlot.slotID = i;
            inventorySlot.amount.text = "";
        }
    }

    public void AddItem(Item itemToAdd, int addAmount = 1)
    {
        ItemType itemToAddType = itemToAdd.itemType;
        switch (itemToAddType)
        {
            case ItemType.CONSUMABLE:
                AddItemToFreeSlot(itemToAdd, addAmount);
                break;
            case ItemType.CURRENCY:
                AddMicelium(addAmount);
                break;
        }

        PlayerController playerController = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<PlayerController>();
        int itemTotal = GetItemTotal(itemToAdd);
        playerController.CheckQuestsItem(itemToAdd, itemTotal);
    }

    public int GetItemTotal(Item itemToCheck)
    {
        int returnValue = 0;

        if (itemToCheck.itemType == ItemType.CURRENCY)
        {
            return miceliumAmount;
        }

        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.containedItem.item == null || inventorySlot.containedItem.item != itemToCheck)
            {
                continue;
            }
            returnValue += inventorySlot.containedItem.itemAmount;
        }

        return returnValue;
    }

    public void AddMicelium(int addAmount)
    {
        miceliumAmount += addAmount;
    }

    public bool RemoveMicelium(int removeAmount)
    {
        if (miceliumAmount - removeAmount < 0)
        {
            return false;
        }
        miceliumAmount = Mathf.Max(0, miceliumAmount - removeAmount);
        return true;
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

    public void CheckMergeability(int giverSlotID, int takerSlotID)
    {
        InventorySlot giverSlot = GetInventorySlotByID(giverSlotID);
        InventorySlot takerSlot = GetInventorySlotByID(takerSlotID);

        if (giverSlot.containedItem.item == takerSlot.containedItem.item || takerSlot.containedItem.item == null)
        {
            MergeSlots(giverSlot, takerSlot);
        }
        else
        {
            SwapSlots(giverSlot, takerSlot);
        }
    }

    public void MergeSlots(InventorySlot giverSlot, InventorySlot takerSlot)
    {
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

    public void SwapSlots(InventorySlot giverSlot, InventorySlot takerSlot)
    {
        Item takerTempItem = takerSlot.containedItem.item;
        int takerTempAmount = takerSlot.containedItem.itemAmount;

        takerSlot.SetItem(giverSlot.containedItem.item, giverSlot.containedItem.itemAmount);
        giverSlot.SetItem(takerTempItem, takerTempAmount);
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
