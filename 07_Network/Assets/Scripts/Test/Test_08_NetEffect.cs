using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_NetEffect : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        NetPlayer player = GameManager.Inst.Player;
        player.IsEffectOn = !player.IsEffectOn;
    }


}
