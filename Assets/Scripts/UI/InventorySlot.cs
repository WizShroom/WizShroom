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
    public TextMeshProUGUI amount;
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

        UpdateName(itemToAdd.itemName);
        UpdateSprite(itemToAdd.itemSprite);
        containedItem.item = itemToAdd;
        int amountToStore = newAmount > itemToAdd.itemCap ? itemToAdd.itemCap : newAmount;
        UpdateAmount(amountToStore);

        return returnAmount;
    }

    public int Additem(int amount)
    {
        int returnAmount = amount;

        int storedAmount = containedItem.itemAmount;
        int newAmount = storedAmount + returnAmount;
        returnAmount = Mathf.Max(0, newAmount - containedItem.item.itemCap);

        int amountToStore = newAmount > containedItem.item.itemCap ? containedItem.item.itemCap : newAmount;
        UpdateAmount(amountToStore);

        return returnAmount;
    }

    public void RemoveItem(int removeAmount = 1, bool removeCompletely = false)
    {
        if (containedItem.itemAmount - removeAmount <= 0 || removeCompletely)
        {
            UpdateName("");
            UpdateSprite(null);
            containedItem.item = null;
            UpdateAmount(0);
        }
        else
        {

            int remainingAmount = containedItem.itemAmount - removeAmount;
            UpdateAmount(remainingAmount);
        }
    }

    public void SetItem(Item itemToSet, int amountToSet = 1)
    {
        UpdateName(itemToSet.itemName);
        UpdateSprite(itemToSet.itemSprite);
        containedItem.item = itemToSet;
        UpdateAmount(amountToSet);
    }

    public void UpdateSprite(Sprite spriteToSet)
    {
        containedItem.itemSprite = spriteToSet;
        slotImage.sprite = spriteToSet;
    }

    public void UpdateName(string nameToSet)
    {
        containedItem.itemName = nameToSet;
    }

    public void UpdateAmount(int amountToSet)
    {
        containedItem.itemAmount = amountToSet;
        string textAmount = containedItem.itemAmount > 0 ? containedItem.itemAmount.ToString() : "";
        amount.text = textAmount;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Use the event data raycast results to see if we hit another slot
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            InventorySlot otherSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();
            if (otherSlot != null && otherSlot != this && otherSlot.CompareTag("InventorySlot"))
            {
                Inventory.Instance.CheckMergeability(slotID, otherSlot.slotID);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (containedItem.item == null)
        {
            return;
        }
        PlayerController playerController = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<PlayerController>();
        if (containedItem.item.UseItem(playerController))
        {
            RemoveItem();
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
