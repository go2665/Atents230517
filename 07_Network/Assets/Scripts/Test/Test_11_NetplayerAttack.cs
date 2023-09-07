using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_11_NetplayerAttack : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        //GameManager.Inst.PlayerRespawn();
        GameManager.Inst.Player.Die();
    }
}
