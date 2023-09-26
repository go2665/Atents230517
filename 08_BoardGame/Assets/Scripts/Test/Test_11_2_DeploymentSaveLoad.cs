using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_11_2_DeploymentSaveLoad : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        bool result = GameManager.Inst.SaveShipDeployData(GameManager.Inst.UserPlayer);
        if (result)
        {
            Debug.Log("저장 성공");
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        bool result = GameManager.Inst.LoadShipDeployData(GameManager.Inst.UserPlayer);
        if (result)
        {
            Debug.Log("로딩 성공");
        }
        else
        {
            Debug.Log("로딩 실패");
        }
    }
}

// 실습 
// 1. 1번을 누르면 현재 배치 상태가 저장된다.
// 2. 2번을 누르면 저장되어 있던 상태로 로딩 되어야 한다.
