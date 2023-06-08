using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Spawner : TestBase
{
    public OldSpawner spawner;


    protected override void Test1(InputAction.CallbackContext context)
    {
        Test1111();
        Debug.Log("테스트1");
        Test1111();
        spawner.TestSpawn();
    }


    void Test1111()
    {
        int i = 0;
        i++;

        return;
    }
}
