using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : EventTrigger
{
    public int slotID;
    public Image slotImage;
    public InventoryItem containedItem = new InventoryItem();

    public bool ContainItem(Item itemToCheck = null)
    {
        if (containedItem.item == null)
        {
            return false;
        }

        if (containedItem.item != itemToCheck)
        {
            return true;
        }

        if (!containedItem.item.canItemStack)
        {
            return true;
        }

        if (containedItem.itemAmount >= containedItem.item.itemCap)
        {
            return true;
        }

        return false;
    }

    public int AddItem(Item itemToAdd, int amount = 1)
    {
        int returnAmount = amount;
        if (ContainItem(itemToAdd))
        {
            return returnAmount;
        }

        int storedAmount = containedItem.itemAmount;
        int newAmount = storedAmount + returnAmount;
        returnAmount = Mathf.Max(0, newAmount - itemToAdd.itemCap);

        containedItem.itemName = itemToAdd.itemName;
        UpdateSprite(itemToAdd.itemSprite);
        containedItem.item = itemToAdd;
        containedItem.itemAmount = newAmount > itemToAdd.itemCap ? itemToAdd.itemCap : newAmount;

        return returnAmount;
    }

    public int Additem(int amount)
    {
        int returnAmount = amount;

        int storedAmount = containedItem.itemAmount;
        int newAmount = storedAmount + returnAmount;
        returnAmount = Mathf.Max(0, newAmount - containedItem.item.itemCap);

        containedItem.itemAmount = newAmount > containedItem.item.itemCap ? containedItem.item.itemCap : newAmount;

        return returnAmount;
    }

    public void RemoveItem(int removeAmount = 1, bool removeCompletely = false)
    {
        if (containedItem.itemAmount - removeAmount <= 0 || removeCompletely)
        {
            containedItem.itemName = "";
            UpdateSprite(null);
            containedItem.item = null;
            containedItem.itemAmount = 0;
        }
        else
        {
            containedItem.itemAmount -= removeAmount;
        }
    }

    public void SetItem(Item itemToSet, int amountToSet = 1)
    {
        containedItem.itemName = itemToSet.itemName;
        UpdateSprite(itemToSet.itemSprite);
        containedItem.item = itemToSet;
        containedItem.itemAmount = amountToSet;
    }

    public void UpdateSprite(Sprite spriteToSet)
    {
        containedItem.itemSprite = spriteToSet;
        slotImage.sprite = spriteToSet;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Use the event data raycast results to see if we hit another slot
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            InventorySlot otherSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();
            if (otherSlot != null && otherSlot != this && otherSlot.CompareTag("InventorySlot"))
            {
                Inventory.Instance.MergeSlots(slotID, otherSlot.slotID);
            }
        }
    }
}

[System.Serializable]
public struct InventoryItem
{
    public string itemName;
    public Sprite itemSprite;
    public Item item;
    public int itemAmount;
}
