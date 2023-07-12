using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeSpawn : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        foreach (Slime slime in slimes)
        {
            slime.Die();
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        if(slimes.Length > 0)
        {
            slimes[0].Die();
        }
    }
}
