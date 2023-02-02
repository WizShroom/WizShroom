using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Collectable : MonoBehaviour
{
    public Item itemToCollect;
    public int containedItemAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItemToFreeSlot(itemToCollect, containedItemAmount);
            Destroy(gameObject);
        }
    }
}