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