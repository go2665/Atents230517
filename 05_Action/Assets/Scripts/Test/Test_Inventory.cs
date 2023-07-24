using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public uint size = 6;
    public ItemCode code = ItemCode.Ruby;
    public uint index = 0;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(null, size);
        inven.AddItem(code, 0);
        inven.AddItem(code, 0);
        inven.AddItem(code, 0);
        inven.AddItem(code, 1);
        inven.AddItem(code, 1);
        inven.AddItem(code, 2);
        inven.AddItem(code, 3);
        inven.AddItem(code, 4);
        inven.AddItem(code, 5);
        inven.PrintInventory();
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
