using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Ranking : TestBase
{
    public RankPanel rankPanel;

    protected override void Test1(InputAction.CallbackContext context)
    {
        rankPanel.TestSave();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        rankPanel.TestLoad();  
    }
}
