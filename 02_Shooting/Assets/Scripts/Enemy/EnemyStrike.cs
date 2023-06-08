using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrike : EnemyBase
{
    [Header("대기했다가 돌진하는 적 데이터")]
    /// <summary>
    /// 기다린 후에 돌진하는 속도
    /// </summary>
    public float secondSpeed = 10.0f;

    /// <summary>
    /// 처음 기다릴 때까지의 시간
    /// </summary>
    public float appearTime = 0.5f;

    /// <summary>
    /// 멈춰서서 대기하는 시간
    /// </summary>
    public float waitTime = 5.0f;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        StopAllCoroutines();
        StartCoroutine(AppearProcess());
    }

    IEnumerator AppearProcess()
    {
        yield return new WaitForSeconds(appearTime);
        speed = 0.0f;
        yield return new WaitForSeconds(waitTime);
        speed = secondSpeed;
    }
}
