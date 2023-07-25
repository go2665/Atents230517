using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_InventoryUI : TestBase
{
    public InventoryUI inventoryUI;
    public uint size = 6;
    public ItemCode code = ItemCode.Ruby;
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
        inven.AddItem(ItemCode.Spaphire, 2);
        inven.AddItem(ItemCode.Spaphire, 2);
        
        inven.PrintInventory();

        inventoryUI.InitializeInventory(inven);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        inven.AddItem(code);
        inven.PrintInventory();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        inven.AddItem(code, index);
        inven.PrintInventory();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        inven.RemoveItem(index);
        inven.PrintInventory();
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        inven.ClearSlot(index);
        inven.PrintInventory();
    }

    protected override void Test5(InputAction.CallbackContext context)
    {
        inven.ClearInventory();
        inven.PrintInventory();
    }
}
