using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_Rank : TestBase
{
    public TabSubPanel tabSubPanel_Action;
    public TabSubPanel tabSubPanel_Time;

    protected override void Test1(InputAction.CallbackContext context)
    {
        RankDataManager rank = GameManager.Inst.RankDataManager;
        rank.Test_ActionRankSetting();

        tabSubPanel_Action.Refresh();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        RankDataManager rank = GameManager.Inst.RankDataManager;
        rank.Test_TimeRankSetting();

        tabSubPanel_Time.Refresh();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        RankDataManager rank = GameManager.Inst.RankDataManager;
        rank.Test_ActionRankSetting();
        rank.Test_TimeRankSetting();
    }
}

// 1. RankPanel은 게임 상태가 Ready가 되었을 때 Close 함수를 실행 시킨다.
// 2. RankPanel은 게임 상태가 Clear가 되었을 때 Open 함수를 실행 시틴다.
// 3. RankPanel에 속해있는 탭 버튼을 누르면
//  3.1. 해당 탭의 버튼만 활성화된다. 그리고 다른 탭의 버튼은 비활성화 된다.(버튼의 색상 변경만)
//  3.2. 해당 탭의 서브패널이 열린다. 그리고 열려있던 다른 탭의 서브패널은 닫힌다.
//    (무조건 하나의 탭만 선택되고 서브패널이 열려야 한다.)
// 4. ToggleButton이 On일 때 누르면 현재 선택된 탭이 닫힌다.
//    ToggleButton이 Off일 때 누르면 현재 선택되어 있던 탭이 열린다.
// 5. ToggleButton이 Off일 때 탭 버튼이 눌려지면 On 상태로 변경된다.