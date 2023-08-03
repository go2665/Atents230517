using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemEquip : TestBase
{
    Player player;

    private void Start()
    {
        Vector3 pos = new Vector3 (0, 0, 0);
        //ItemFactory.MakeItem(ItemCode.IronSword, pos);
        ItemFactory.MakeItem(ItemCode.SilverSword, pos);
        ItemFactory.MakeItem(ItemCode.OldSword, pos);
        ItemFactory.MakeItem(ItemCode.RoundShield, pos);
        ItemFactory.MakeItem(ItemCode.KiteShield, pos);

        player = GameManager.Inst.Player;
        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory.Test_ItemEquip(0);
    }
}