using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Base Item Properties")]
    public Sprite itemIcon;
    public string itemName;
    public InventoryType itemType;
}

[System.Serializable]
public class Equipment
{
    public InventoryType type;
    public Sprite icon;
    public Item item;

    public Equipment(InventoryType type = InventoryType.Item, Sprite icon = null, Item item = null)
    {
        this.type = type;
        this.icon = icon;
        this.item = item;
    }
}

public enum InventoryType
{
    Item,
    Weapon,
    Armor,
    Accessory,
    Money,
    StatBoost,
    ProjectileUpgrader,
}