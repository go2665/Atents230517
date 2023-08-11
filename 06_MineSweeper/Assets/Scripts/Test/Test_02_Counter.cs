using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Counter : TestBase
{
    public GameManager.GameState state = GameManager.GameState.Ready;

    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.Test_Flag(GameManager.Inst.FlagCount + 1);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.Test_Flag(GameManager.Inst.FlagCount - 1);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.Inst.Test_State(state);
    }
}
