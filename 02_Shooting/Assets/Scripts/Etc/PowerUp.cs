using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PooledObject
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 방향 전환되는 시간 간격
    /// </summary>
    public float dirChangeInterval = 1.0f;

    /// <summary>
    /// 방향 전환 최대 갯수
    /// </summary>
    public int dirChangeCountMax = 5;

    /// <summary>
    /// 현재 남아있는 방향 전환 갯수
    /// </summary>
    int dirChangeCount;

    /// <summary>
    /// 방향 전환 갯수 변경용 프로퍼티
    /// </summary>
    int DirChangeCount
    {
        get => dirChangeCount;
        set
        {            
            dirChangeCount = value;
            anim.SetInteger("Count", dirChangeCount);

            StopAllCoroutines();        // 이전 코루틴은 우선 모두 정지(인터벌 간격으로 방향 전환하던 것 취소용도)

            if ( dirChangeCount > 0 )   // 튕길 횟수가 남아있으면    
            {
                StartCoroutine(DirChange());    // 인터벌 이후에 다시 방향 전환
            }
            //Debug.Log($"DirChangeCount : {dirChangeCount}");
        }
    }

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector2 dir;

    Animator anim;

    IEnumerator DirChange()
    {
        yield return new WaitForSeconds(dirChangeInterval); // 우선 기다리고
        dir = Random.insideUnitCircle;  // 랜덤 방향 정하기
        dir.Normalize();                // 방향벡터를 유닛벡터로 변경해서 방향만 남기기

        DirChangeCount--;               // 카운트 감소 시키면서 다음 코루틴 실행 예약
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        DirChangeCount = dirChangeCountMax;
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( DirChangeCount > 0 && collision.gameObject.CompareTag("Border"))
        {
            // 튕길 갯수가 남아있고 보더와 부딪치면 반사
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
            DirChangeCount--;
        }
    }
}
