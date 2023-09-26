using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_11_ShipDeploymentScene : TestBase
{
    public GameState state = GameState.ShipDeployment;
    public DeploymentToggle toggle;

    GameManager manager;

    private void Start()
    {
        manager = GameManager.Inst;
        //manager.GameState = state;
    }

    private void OnValidate()
    {
        if(manager != null)
        {
            manager.GameState = state;
        }
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        toggle.Test_State(0);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        toggle.Test_State(1);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        toggle.Test_State(2);
    }
}