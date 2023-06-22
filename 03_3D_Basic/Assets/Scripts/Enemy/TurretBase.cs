using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 1.0f;

    /// <summary>
    /// 발사할 총알의 종류
    /// </summary>
    public PoolObjectType projectileType = PoolObjectType.Bullet;

    /// <summary>
    /// 총몸의 트랜스폼(총구 방향 돌리는 것이 목적)
    /// </summary>
    protected Transform barrelBodyTransform;

    /// <summary>
    /// 총알이 발사되는 위치 표현용
    /// </summary>
    protected Transform fireTransform;

    /// <summary>
    /// 주기적으로 총알을 발사하는 코루틴을 저장해 놓은 변수
    /// </summary>
    protected IEnumerator fireCoroutine;

    protected virtual void Awake()
    {
        barrelBodyTransform = transform.GetChild(2);        // 총몸 찾기
        fireTransform = barrelBodyTransform.GetChild(1);    // 발사 위치 찾기
        fireCoroutine = PeriodFire();                       // 코루틴 저장
    }

    /// <summary>
    /// 주기적으로 총알을 발사하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator PeriodFire()
    {
        while(true)
        {
            OnFire();       // 한번 발사하고
            yield return new WaitForSeconds(fireInterval);  // 잠깐 대기
        }
    }

    /// <summary>
    /// 총알을 한번 발사하는 함수. 상속받은 곳에서 별도 구현 권장.
    /// </summary>
    protected virtual void OnFire()
    {
        Factory.Inst.GetObject(projectileType, fireTransform.position);
    }
}
