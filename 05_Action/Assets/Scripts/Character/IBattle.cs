using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    /// <summary>
    /// 이 오브젝트의 트랜스폼(구현 필요 없음)
    /// </summary>
    Transform transform { get; }

    /// <summary>
    /// 공격력 확인용 프로퍼티
    /// </summary>
    float AttackPower { get; }

    /// <summary>
    /// 방어력 확인용 프로퍼티
    /// </summary>
    float DefencePower { get; }

    /// <summary>
    /// 공격 함수
    /// </summary>
    /// <param name="target">내가 공격할 대상</param>
    void Attack(IBattle target);

    /// <summary>
    /// 방어 함수
    /// </summary>
    /// <param name="damage">내가 받은 데미지</param>
    void Defence(float damage);
}
