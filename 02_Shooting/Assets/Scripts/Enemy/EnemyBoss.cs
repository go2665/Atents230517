using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [Header("보스 데이터")]
    /// <summary>
    /// 보스 활동 영역(최소)
    /// </summary>
    public Vector2 areaMin = new Vector2(2, -3);

    /// <summary>
    /// 보스 활동 영역(최대)
    /// </summary>
    public Vector2 areaMax = new Vector2(7, 3);

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float bulletInterval = 1.0f;

    /// <summary>
    /// 총알용 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 추적용 미사일 프리팹
    /// </summary>
    public GameObject missilePrefab;

    Vector3 targetPosition;
    Vector3 moveDirection;
    Transform firePosition1;
    Transform firePosition2;
    Transform firePosition3;

    protected override void Awake()
    {
        base.Awake();
        firePosition1 = transform.GetChild(2);
        firePosition2 = transform.GetChild(3);
        firePosition3 = transform.GetChild(4);
    }

    public override void OnInitialize()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = 0.0f;
        transform.position = newPosition;   // y위치는 0으로 고정

        StopAllCoroutines();                // 코루틴 모두 정지
        StartCoroutine(AppearProcess());    // 등장 프로세스용 코루틴 실행
    }

    IEnumerator AppearProcess()
    {
        float oldSpeed = speed;         // 기존 속도 저장하고
        speed = 0.0f;                   // 속도를 0으로 해서 안움직이게 만듬
        float remainDistance = 5;       // 남아있는 거리는 5

        while (remainDistance > 0.1f)   // 남아있는 거리가 거의 0이 될 때까지 반복
        {
            // deltaMove = (0에서 remainDistance까지 중에 (Time.deltaTime * 0.5f)만큼 진행된 위치 )
            float deltaMove = Mathf.Lerp(0, remainDistance, Time.deltaTime * 1.2f); 
            remainDistance -= deltaMove;                        // deltaMove만큼 남은 거리 감소
            transform.Translate(deltaMove * (-Vector3.right));  // deltaMove만큼 왼쪽으로 이동
            yield return null;
        }
        speed = oldSpeed;
        SetNextTargetPosition();        // 다음 위치 결정

        StartCoroutine(BulletFire());   // 총알 발사 시작
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * speed * moveDirection);

        //if(targetPosition == transform.position)    // 절대 하면 안되는 코드(float은 오차가 있기 때문에 직접비교는 하면 안됨)

        //if( (targetPosition - transform.position).sqrMagnitude < 0.0001f )    // 목적지와 내 위치의 거리가 일정 이하면
        //{
        //    // 목적지에 거의 도착했다.
        //    SetNextTargetPosition();  // 새 목적지 설정
        //}

        if (transform.position.y > areaMax.y)       // 내 위치가 최대치보다 위면
        {
            SetNextTargetPosition();    // 새 목적지 설정
            StartCoroutine(MissileFire());
        }
        else if(transform.position.y < areaMin.y)   // 내 위치가 최소치보다 아래면
        {
            SetNextTargetPosition();    // 새 목적지 설정
            StartCoroutine(MissileFire());
        }
    }

    /// <summary>
    /// 다음 targetPosition구하는 함수
    /// </summary>
    void SetNextTargetPosition()
    {
        float x;
        float y;

        x = Random.Range(areaMin.x, areaMax.x);
        if (transform.position.y > 0)
        {
            y = areaMin.y;
        }
        else
        {
            y = areaMax.y;
        }

        targetPosition = new Vector3(x, y);
        moveDirection = targetPosition - transform.position;    // 방향 구하고
        moveDirection.Normalize();  // 길이를 1로 만들기(정규화시킴. Normalize)
    }

    IEnumerator BulletFire()
    {
        while (true)
        {
            // bulletPrefab를 firePosition1.position위치에 Quaternion.identity만큼 회전시켜서 생성
            Instantiate(bulletPrefab, firePosition1.position, Quaternion.identity);
            Instantiate(bulletPrefab, firePosition2.position, Quaternion.identity);
            yield return new WaitForSeconds(bulletInterval);
        }
    }

    IEnumerator MissileFire()
    {
        for(int i=0;i<3;i++)
        {
            GameObject obj = Instantiate(missilePrefab, firePosition3.position, Quaternion.identity);
            EnemyBase enemy = obj.GetComponent<EnemyBase>();
            enemy.OnInitialize();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
