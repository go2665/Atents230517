using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Spawner : TestBase
{
    public Spawner spawner;


    protected override void Test1(InputAction.CallbackContext context)
    {
        Debug.Log("테스트1");
        spawner.TestSpawn();
    }
}
