using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerHP : TestBase
{
    Player player;
    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        player.HP -= 17;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        player.HP += 11;
    }
}
