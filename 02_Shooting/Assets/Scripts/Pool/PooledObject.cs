using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀에 들어갈 오브젝트들이 상속받을 클래스
/// </summary>
public class PooledObject : MonoBehaviour
{
    /// <summary>
    /// 이 게임 오브젝트가 비활성화 될 때 실행되는 델리게이트
    /// </summary>
    public Action onDisable;

    protected virtual void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();    // 비활성화 되었다고 알림
    }

    /// <summary>
    /// 일정 시간 후에 이 게임오브젝트를 비활성화 시키는 코루틴
    /// </summary>
    /// <param name="delay">비활성화가 될때까지 걸리는 시간(기본 = 0.0f)</param>
    /// <returns></returns>
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay만큼 대기하고
        gameObject.SetActive(false);            // 게임 오브젝트 비활성화
    }
}
