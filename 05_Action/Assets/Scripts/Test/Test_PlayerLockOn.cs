using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerLockOn : TestBase
{
    public Enemy[] dummies;

    protected override void Test1(InputAction.CallbackContext context)
    {
        dummies[0].HP -= 10000;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        dummies[1].HP -= 10000;
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        dummies[2].HP -= 10000;
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        Player player = GameManager.Inst.Player;
        player.HP -= 10000;
    }

    protected override void Test5(InputAction.CallbackContext context)
    {
        Player player = GameManager.Inst.Player;
        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory.AddItem(ItemCode.OldSword);
        player.Inventory.AddItem(ItemCode.RoundShield);
        player.Inventory.AddItem(ItemCode.KiteShield);
    }
}
