using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : PooledObject
{
    [Header("Base 데이터")]
    /// <summary>
    /// 현재 이동하는 속도
    /// </summary>
    public float speed = 1.0f;

    // [SerializeField]
    /// <summary>
    /// 이 적이 주는 점수
    /// </summary>
    public int score = 10;

    /// <summary>
    /// 점수 확인용 프로퍼티
    /// </summary>
    public int Score => score;

    public int MaxHP = 1;
    int hp = 1;
    public int HP
    {
        get => hp;
        protected set
        {
            if( hp != value )
            {
                hp = value;
                if(hp <= 0 )
                {
                    hp = 0;
                    Die();
                }
            }
        }
    }

    /// <summary>
    /// 죽었을 때 실행되는 델리게이트
    /// </summary>
    public Action<int> onDie;

    /// <summary>
    /// 죽었을 때 점수를 줄 플레이어
    /// </summary>
    Player targetPlayer = null;

    protected virtual void Awake()
    {
        hp = MaxHP;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();
    }

    protected override void OnDisable()
    {
        if (targetPlayer != null)
        {
            onDie -= targetPlayer.AddScore;

            onDie = null;
            targetPlayer = null;
        }
        base.OnDisable();
    }

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);        // 각 클래스별 이동 업데이트 함수 실행
    }

    /// <summary>
    /// 업데이트에서 실행되는 이동 처리 함수
    /// </summary>
    /// <param name="deltaTime">프레임간 경과시간</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * speed * -transform.right, Space.World);  // 그냥 왼쪽으로만 이동하기
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))   // 총알만 충돌 처리
        {
            HP--;
        }
    }

    /// <summary>
    /// 클래스별 초기화 함수
    /// </summary>
    protected virtual void OnInitialize()
    {
        if(targetPlayer == null)
        {
            targetPlayer = GameManager.Inst.Player;
        }

        if(targetPlayer != null)
        {
            onDie += targetPlayer.AddScore;
        }
    }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    protected virtual void Die()
    {
        // 터지는 이팩트 생성
        Factory.Inst.GetObject(PoolObjectType.Explosion, 
            transform.position, UnityEngine.Random.Range(0.0f, 360.0f));

        onDie?.Invoke(score);               // 죽었다고 알리기

        gameObject.SetActive(false);        // 오브젝트 삭제
    }
}
