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
        player.MP -= 17;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        player.HP += 11;
        player.MP += 11;
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        player.HealthRegenetate(50, 3);
        player.ManaRegenetate(50, 3);
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        player.HealthRegenerateByTick(10, 0.5f, 5);
        player.ManaRegenerateByTick(10, 0.5f, 5);
    }
}
