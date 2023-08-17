using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04_GameManager : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.GameReset();

        //ResetButton.ButtonState.Normal
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.GameClear();
    }
}
