using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory2 : TestBase
{
    public uint size = 6;
    public ItemCode code = ItemCode.Ruby;
    public uint from = 0;
    public uint to = 1;
    public uint splitCount = 1;
    public ItemSortBy sortBy = ItemSortBy.Code;
    public bool isAcending = true;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(null, size);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 1);
        inven.AddItem(ItemCode.Emerald, 2);
        inven.AddItem(ItemCode.Spaphire, 3);
        
        inven.PrintInventory();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        inven.MoveItem(from, to);
        inven.PrintInventory();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        inven.SplitItem(from, splitCount);
        inven.PrintInventory();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        inven.MoveItem(Inventory.TempSlotIndex, to);
        inven.PrintInventory();
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        inven.SlotSorting(sortBy, isAcending);
        inven.PrintInventory();
    }
}
