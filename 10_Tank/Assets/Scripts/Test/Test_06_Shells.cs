using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Shells : TestBase
{
    public Transform fireTransform;

    protected override void Test1(InputAction.CallbackContext context)
    {
        //Factory.Inst.GetNormalShell(fireTransform);        
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        //Factory.Inst.GetGuidedShell(fireTransform);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        //Factory.Inst.GetClustShell(fireTransform);

    }
}

// 실습
// 1. 탱크에 포탄 발사 쿨타임 추가
//  1.1. UI 추가(탱크 위쪽에 동그란 fill타입으로 생성). 빌보드이어야 한다.
// 2. 하늘에서 아이템이 랜덤하게 드랍됨
//  2.1. 습득한 아이템 종류에 따라 포탄이 변경됨
//  2.2. 8~12초 간격으로 아이템이 하나씩 떨어짐
