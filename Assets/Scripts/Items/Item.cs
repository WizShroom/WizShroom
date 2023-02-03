using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int itemCap = 99;
    public bool canItemStack = true;

    public ItemType itemType;

    public virtual bool UseItem(MobController mobController)
    {
        Debug.Log("Clicked item");
        return false;
    }
}

public enum ItemType
{
    CONSUMABLE,
    CURRENCY,
}
