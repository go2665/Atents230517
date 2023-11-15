using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Shells : TestBase
{
    public Transform fireTransform;

    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetShell(fireTransform);        
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Factory.Inst.GetGuided(fireTransform);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        Factory.Inst.GetClust(fireTransform);
    }
}
