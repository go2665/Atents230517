using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = 5.0f;

    public override void OnPlayerTurnStart(int _)
    {
        base.OnPlayerTurnStart(_);

        // thinkingTimeMin ~ thinkingTimeMax 사이에 랜덤으로 공격
    }
}
