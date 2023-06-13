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
            if( dirChangeCount < 1 )    // 튕길 갯수가 0이하면 모든 코루틴 정지(=>시간 변화에 따라 튕기는 것 정지)
            {
                StopAllCoroutines();
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
        while(true)
        {
            yield return new WaitForSeconds(dirChangeInterval);
            dir = Random.insideUnitCircle;
            dir.Normalize();
            DirChangeCount--;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        dirChangeCount = dirChangeCountMax;

        StopAllCoroutines();
        StartCoroutine(DirChange());
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
