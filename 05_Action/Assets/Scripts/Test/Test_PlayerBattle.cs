using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class Test_PlayerBattle : TestBase
{
    Player player;

    private void Start()
    {
        //Vector3 pos = new Vector3 (0, 0, -2.0f);
        //ItemFactory.MakeItem(ItemCode.IronSword, pos);
        //ItemFactory.MakeItem(ItemCode.SilverSword, pos);
        //ItemFactory.MakeItem(ItemCode.OldSword, pos);
        //ItemFactory.MakeItem(ItemCode.RoundShield, pos);
        //ItemFactory.MakeItem(ItemCode.KiteShield, pos);        
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        if (player == null)
        {
            player = GameManager.Inst.Player;
        }
        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory.AddItem(ItemCode.OldSword);
        player.Inventory.AddItem(ItemCode.RoundShield);
        player.Inventory.AddItem(ItemCode.KiteShield);        
        //player.Inventory.Test_ItemEquip(0);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Time.timeScale = 1;
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        if(player == null)
        {
            player = GameManager.Inst.Player;
        }
        player.HP -= player.MaxHP;
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        if (player == null)
        {
            player = GameManager.Inst.Player;
        }
        player.Defence(70);
    }
}