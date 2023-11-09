using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_Tank : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Time.timeScale = 0.1f;
    }
}
