using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    /// <summary>
    /// fill의 스케일 기준이 될 피봇용 transform
    /// </summary>
    Transform fillPivot;

    private void Awake()
    {
        fillPivot = transform.GetChild(1);

        IHealth target = GetComponentInParent<IHealth>();
        target.onHealthChange += Refresh;   // HP변경시 Refresh 실행시키기
    }

    /// <summary>
    /// 적의 HP가 변경되면 실행될 함수
    /// </summary>
    /// <param name="ratio">적 HP/MaxHP</param>
    private void Refresh(float ratio)
    {
        fillPivot.localScale = new Vector3(ratio, 1, 1);    // 로컬 스케일 조정해서 비율 표시
    }

    /// <summary>
    /// 모든 게임오브젝트의 Update가 끝난 후 호출되는 함수
    /// </summary>
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;    // 카메라의 회전과 일치시키기
    }
}
