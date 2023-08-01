using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    /// <summary>
    /// HP 확인 및 설정용 프로퍼티
    /// </summary>
    float HP { get; set; }

    /// <summary>
    /// 최대 HP 확인용 프로퍼티
    /// </summary>
    float MaxHP { get; }

    /// <summary>
    /// HP가 변경될 때마다 실행될 델리게이트(파라메터는 비율) 용 프로퍼티
    /// </summary>
    Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    void Die();

    /// <summary>
    /// 사망을 알리기 위한 델리게이트 용 프로퍼티
    /// </summary>
    Action onDie { get; set; }

    /// <summary>
    /// 생존을 확인하기 위한 프로퍼티
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// 체력을 지속적으로 증가시켜 주는 함수. 초당 totalRegen/duration만큼 회복
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복 시간</param>
    void HealthRegenetate(float totalRegen, float duration);
}
