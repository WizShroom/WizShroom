using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreenType : UITypeController
{
    public Transform inventorySlotContainer;

    public InventoryItemOverlay inventoryItemOverlay;

    public List<GameObject> availableScreens;

    private void Awake()
    {
        foreach (GameObject availableScreen in availableScreens)
        {
            availableScreen.SetActive(false);
        }

        availableScreens[0].SetActive(true);
    }

    public void EnableScreen(int screenID)
    {
        foreach (GameObject availableScreen in availableScreens)
        {
            availableScreen.SetActive(false);
        }

        availableScreens[screenID].SetActive(true);
    }
}