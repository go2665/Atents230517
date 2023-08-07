using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerBattle : TestBase
{
    Player player;

    private void Start()
    {
        Vector3 pos = new Vector3 (0, 0, -2.0f);
        ItemFactory.MakeItem(ItemCode.IronSword, pos);
        ItemFactory.MakeItem(ItemCode.SilverSword, pos);
        ItemFactory.MakeItem(ItemCode.OldSword, pos);
        ItemFactory.MakeItem(ItemCode.RoundShield, pos);
        ItemFactory.MakeItem(ItemCode.KiteShield, pos);        
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        player = GameManager.Inst.Player;
        player.Inventory.Test_ItemEquip(0);
    }
}