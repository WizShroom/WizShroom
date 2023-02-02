using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public InventorySlotContent inventorySlotContent;

}

[System.Serializable]
public class InventorySlotContent
{
    public Sprite inventoryIcon;
    public Equipment itemEquipment;
}