using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryPanel;
    bool activeInventory = false;

    private void Start()
    {
        InventoryPanel.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeInventory)
            {
                activeInventory = false;
                InventoryPanel.SetActive(activeInventory);
            }
        }
    }
}
