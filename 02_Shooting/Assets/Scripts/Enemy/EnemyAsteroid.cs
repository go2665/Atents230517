using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    [Header("운석 데이터")]
    /// <summary>
    /// 최소 이동 속도
    /// </summary>
    public float minMoveSpeed = 2.0f;
    /// <summary>
    /// 최대 이동 속도
    /// </summary>
    public float maxMoveSpeed = 4.0f;

    /// <summary>
    /// 최소 회전 속도
    /// </summary>
    public float minRotateSpeed = 30.0f;

    /// <summary>
    /// 최대 회전 속도
    /// </summary>
    public float maxRotateSpeed = 360.0f;

    /// <summary>
    /// 현재 회전 속도
    /// </summary>
    float rotateSpeed = 0.0f;

    /// <summary>
    /// 이동 방향
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// 목적지
    /// </summary>
    private Vector3? destination = null;
    
    /// <summary>
    /// 목적지 확인 및 설정용 프로퍼티
    /// </summary>
    public Vector3? Destination
    {
        get => destination;
        set
        {
            if (destination == null)    // destination은 null일 때만 세팅된다. (한번만 설정 가능하다)
            {
                destination = value;
                direction = (destination.Value - transform.position).normalized;    // 벡터의 크기를 1로 만들기(방향만 남겨 놓기)
                //Debug.Log($"목적지 : {destination}");
            }
        }
    }

    /// <summary>
    /// 폭발적으로 미니운석이 생성될 확률
    /// </summary>
    [Range(0f, 1f)]
    public float criticalRate = 0.95f;

    /// <summary>
    /// 운석의 최소 수명
    /// </summary>
    public float minLifeTime = 4.0f;

    /// <summary>
    /// 운석의 최대 수명
    /// </summary>
    public float maxLifeTime = 7.0f;

    /// <summary>
    /// 원래 점수. 재활용할 때 설정할 점수
    /// </summary>
    private int originalScore = 0;

    protected override void Awake()
    {
        base.Awake();
        originalScore = Score;  // 만들어졌을 때 원래 점수 저장해 놓기
    }

    /// <summary>
    /// 업데이트에서 실행되는 이동 처리 함수
    /// </summary>
    /// <param name="deltaTime">프레임간 경과시간</param>
    protected override void OnMoveUpdate(float deltaTime)
    {
        if (destination != null)
        {
            transform.Translate(deltaTime * speed * direction, Space.World);    // 이동(월드 기준)
        }
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward);            // 회전
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);    // 운석 이동 방향 그리기
    }

    /// <summary>
    /// 클래스별 초기화 함수
    /// </summary>
    protected override void OnInitialize()
    {
        //Debug.Log("OnInitialize");
        base.OnInitialize();

        speed = Random.Range(minMoveSpeed, maxMoveSpeed);           // 속도만 랜덤으로 처리
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);

        score = originalScore;          // 활성화 될 때 점수 복구 시키기

        StartCoroutine(SelfCrush());
    }

    protected override void Die()
    {
        //Debug.Log("Die");
        int count;              // 생성할 자식 갯수

        if( Random.value < criticalRate )   
        { 
            count = 20;                     // 크리티컬이 터지면 자식은 20개 생성
        }
        else
        {
            count = Random.Range(3, 8);     // 정상적인 상황이면 3~7개 생성
        }

        float angle = 360.0f / count;                   // 사이각 구하기
        float startAngle = Random.Range(0.0f, 360.0f);  // 시작각 구하기
        for ( int i=0;i<count; i++ )
        {
            // 생성하면서 위치와 회전도 지정
            Factory.Inst.GetAsteroidMini(transform.position, startAngle + angle * i);
        }

        base.Die();
    }

    IEnumerator SelfCrush()
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        score = 0;      // 임시로 점수를 0으로 만들어서 자폭으로 점수가 안들어오게 만들기
        Die();
    }

    public void Test_Die()
    {
        Die();
    }
}