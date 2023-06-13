using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PowerUp : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetObject(PoolObjectType.PowerUp);
    }
}
