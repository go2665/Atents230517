using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_InventoryUI2 : TestBase
{
    public InventoryUI inventoryUI;
    public ItemSpliterUI spliterUI;
    public uint size = 6;
    public uint index = 0;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(null, size);
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

    protected override void Test1(InputAction.CallbackContext context)
    {
        spliterUI.Open(inven[index]);
    }
}
