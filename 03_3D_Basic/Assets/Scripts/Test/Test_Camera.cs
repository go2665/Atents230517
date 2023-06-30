using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Camera : TestBase
{
    public Player player;

    protected override void Test1(InputAction.CallbackContext context)
    {
        player.Die();
    }
}
