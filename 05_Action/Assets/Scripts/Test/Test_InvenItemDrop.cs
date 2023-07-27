using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Test_InvenItemDrop : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

    void Start()
    {
        inven = new Inventory(null, 6);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Spaphire, 2);
        inven.AddItem(ItemCode.Spaphire, 2);

        inven.PrintInventory();

        inventoryUI.InitializeInventory(inven);
    }

}
