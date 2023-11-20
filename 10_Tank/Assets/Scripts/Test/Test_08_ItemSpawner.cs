using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_ItemSpawner : TestBase
{
    public ItemSpawner spawner;
    protected override void Test1(InputAction.CallbackContext context)
    {
        spawner.Test_Counter();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        spawner.StopSpawner();
    }
}
