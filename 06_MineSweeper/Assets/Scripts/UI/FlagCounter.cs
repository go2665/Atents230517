using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : CounterBase
{
    private void Start()
    {
        GameManager.Inst.onFlagCountChange += Refresh;      // 깃발 개수가 변경되었을 떄 Refresh
        //GameManager.Inst.onGameReady += () => Refresh(GameManager.Inst.FlagCount);  // 게임이 리셋되었을 때 Refresh

        Refresh(GameManager.Inst.FlagCount);
    }
}
