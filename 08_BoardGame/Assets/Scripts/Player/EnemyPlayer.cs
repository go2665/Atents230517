using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = 5.0f;

    protected override void Start()
    {
        base.Start();
        thinkingTimeMax = Mathf.Min(thinkingTimeMax, TurnManager.Inst.TurnDuration);    // 생각하는 시간은 턴이 자동으로 넘어가는 시간보다 클 수 없다.
    }

    protected override void OnPlayerTurnStart(int _)
    {
        base.OnPlayerTurnStart(_);

        // thinkingTimeMin ~ thinkingTimeMax 사이에 랜덤으로 공격
        float delay = Random.Range(thinkingTimeMin, thinkingTimeMax);
        StartCoroutine(AutoStart(delay));
    }

    protected override void OnPlayerTurnEnd()
    {
        StopAllCoroutines();        // AutoStart 코루틴 정지(혹시 턴 종료 후에 실행되는 것 방지)
        base.OnPlayerTurnEnd();
    }

    /// <summary>
    /// 일정 시간 후에 자동으로 공격하는 코루틴
    /// </summary>
    /// <param name="delay">기다릴 시간</param>
    /// <returns></returns>
    IEnumerator AutoStart(float delay)
    {
        yield return new WaitForSeconds(delay); // delay만큼 기다리고
        AutoAttack();                           // 자동 공격
    }

    protected override void OnDefeat()
    {
        StopAllCoroutines();
        base.OnDefeat();
    }
}
