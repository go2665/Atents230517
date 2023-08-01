using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana
{
    /// <summary>
    /// MP 확인 및 설정용 프로퍼티
    /// </summary>
    float MP { get; set; }

    /// <summary>
    /// 최대 MP 확인용 프로퍼티
    /// </summary>
    float MaxMP { get; }

    /// <summary>
    /// MP가 변경될 때마다 실행될 델리게이트(파라메터는 비율) 용 프로퍼티
    /// </summary>
    Action<float> onManaChange { get; set; }

    /// <summary>
    /// 생존을 확인하기 위한 프로퍼티
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// MP를 지속적으로 증가시켜 주는 함수. 초당 totalRegen/duration만큼 회복
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복 시간</param>
    void ManaRegenetate(float totalRegen, float duration);

    /// <summary>
    /// MP를 틱단위로 증가시켜 주는 함수. 전체 회복량 = tickRegen * totalTickCount
    /// </summary>
    /// <param name="tickRegen">틱 당 회복량</param>
    /// <param name="tickTime">한틱간의 시간 간격</param>
    /// <param name="totalTickCount">회복을 수행할 전체 틱수</param>
    void ManaRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount);

}
