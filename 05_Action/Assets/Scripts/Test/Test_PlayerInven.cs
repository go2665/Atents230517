using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerInven : TestBase
{
    public ItemCode itemCode = ItemCode.Ruby;

    protected override void Test1(InputAction.CallbackContext context)
    {
        int index = Random.Range(0, GameManager.Inst.ItemData.length);
        ItemCode code = GameManager.Inst.ItemData[index].code;

        Vector3 pos = Random.insideUnitSphere * 5;
        pos.y = 0.0f;
        ItemFactory.MakeItem(code, pos, true);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Player player = GameManager.Inst.Player;
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Spaphire);
        player.Inventory.AddItem(ItemCode.Spaphire);
        player.Inventory.AddItem(ItemCode.Spaphire);
        player.Inventory.AddItem(ItemCode.Spaphire);

    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        Vector3 pos = Random.insideUnitSphere * 5;
        pos.y = 0.0f;
        ItemFactory.MakeItem(itemCode, pos, true);
    }
}
