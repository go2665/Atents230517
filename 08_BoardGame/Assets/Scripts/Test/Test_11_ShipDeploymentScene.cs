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
        manager.GameState = state;
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


// 실습
// 1. 선택된 배가 없을 때 보드에 배치되어있는 배를 클릭하면 배치가 해제된다.
// 2. 랜덤 배치 버튼을 누르면 아직 배치되지 않은 모든 배가 배치된다.
// 3. 배치 완료 버튼은 interactable이 꺼진 상태로 시작하고 모든 배가 배치 완료되면 interactable이 켜진다.